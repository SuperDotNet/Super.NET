using Super.Compose;
using Super.Model.Selection;
using Super.Model.Selection.Alterations;
using Super.Model.Selection.Conditions;
using Super.Model.Sequences;
using Super.Model.Sequences.Query;
using Super.Reflection.Types;
using Super.Runtime;
using Super.Runtime.Environment;
using Super.Runtime.Invocation;
using System;
using System.Linq;

namespace Super.Aspects
{
	public static class Extensions
	{
		public static ISelect<TIn, TOut> Configured<TIn, TOut>(this ISelect<TIn, TOut> @this)
			=> Aspects<TIn, TOut>.Default.Get(@this).Get(@this);

		public static IAspect<TIn, TOut> Registered<TIn, TOut>(this IAspect<TIn, TOut> @this)
		{
			AspectRegistry.Default.Execute(new Registration<TIn, TOut>(@this));
			return @this;
		}
	}

	public interface IAspect<TIn, TOut> : IAlteration<ISelect<TIn, TOut>> {}

	public sealed class Aspects<TIn, TOut> : Model.Selection.Select<ISelect<TIn, TOut>, IAspect<TIn, TOut>>
	{
		public static Aspects<TIn, TOut> Default { get; } = new Aspects<TIn, TOut>();

		Aspects() : base(Start.A.Selection<ISelect<TIn, TOut>>()
		                      .By.Type.Then()
		                      .Select(SystemStores.New(RegisteredAspects<TIn, TOut>.Default.Stores().New)
		                                          .Assume())) {}
	}

	sealed class RegisteredAspects<TIn, TOut> : ISelect<Type, IAspect<TIn, TOut>>
	{
		public static RegisteredAspects<TIn, TOut> Default { get; } = new RegisteredAspects<TIn, TOut>();

		RegisteredAspects() : this(AspectRegistrations<TIn, TOut>.Default) {}

		readonly ISelect<Type, Array<Type>>              _implementations;
		readonly IArray<Array<Type>, IAspect<TIn, TOut>> _registrations;

		public RegisteredAspects(IArray<Array<Type>, IAspect<TIn, TOut>> registrations)
			: this(SelectionImplementations.Default, registrations) {}

		public RegisteredAspects(ISelect<Type, Array<Type>> implementations,
		                         IArray<Array<Type>, IAspect<TIn, TOut>> registrations)
		{
			_implementations = implementations;
			_registrations   = registrations;
		}

		public IAspect<TIn, TOut> Get(Type parameter)
		{
			var implementations = _implementations.Get(parameter);
			var length          = implementations.Length;
			if (implementations.Length > 0)
			{
				var store = new DynamicStore<IAspect<TIn, TOut>>(32);
				for (var i = 0u; i < length; i++)
				{
					var registrations = _registrations.Get(implementations[i].GenericTypeArguments);
					store = store.Add(new Store<IAspect<TIn, TOut>>(registrations));
				}

				return new CompositeAspect<TIn, TOut>(store.Get().Instance);
			}

			return EmptyAspect<TIn, TOut>.Default;
		}
	}

	public sealed class AspectImplementations : GenericImplementations
	{
		public static AspectImplementations Default { get; } = new AspectImplementations();

		AspectImplementations() : base(typeof(IAspect<,>)) {}
	}

	public sealed class AspectImplementationArguments : GenericImplementationArguments
	{
		public static AspectImplementationArguments Default { get; } = new AspectImplementationArguments();

		AspectImplementationArguments() : base(AspectImplementations.Default) {}
	}

	static class Implementations
	{
		public static Func<object, Array<Type>> Arguments { get; }
			= Start.A.Selection.Of.Any.By.Type.Select(AspectImplementationArguments.Default).Get;

		public static Func<Array<IRegistration>> Registrations { get; }
			= AspectRegistry.Default.Get;
	}

	sealed class AdapterAspects<TIn, TOut> : ISelect<object, IAspect<TIn, TOut>>
	{
		readonly static Array<Type> Types = new Array<Type>(A.Type<TIn>(), A.Type<TOut>());

		public static AdapterAspects<TIn, TOut> Default { get; } = new AdapterAspects<TIn, TOut>();

		AdapterAspects() : this(Implementations.Arguments, Start.A.Generic((typeof(Cast<,,,>)))
		                                                        .Of.Type<IAspect<TIn, TOut>>()
		                                                        .WithParameterOf<object>()) {}

		readonly Func<object, Array<Type>>            _arguments;
		readonly IGeneric<object, IAspect<TIn, TOut>> _generic;

		public AdapterAspects(Func<object, Array<Type>> arguments, IGeneric<object, IAspect<TIn, TOut>> generic)
		{
			_arguments = arguments;
			_generic   = generic;
		}

		public IAspect<TIn, TOut> Get(object parameter)
		{
			var types  = _arguments(parameter).Open().Append(Types.Open()).ToArray();
			var result = _generic.Get(types)(parameter);
			return result;
		}
	}

	sealed class Cast<TI, TO, TIn, TOut> : IAspect<TIn, TOut> where TIn : TI where TOut : TO
	{
		readonly IAspect<TI, TO> _aspect;

		public Cast(IAspect<TI, TO> aspect) => _aspect = aspect;

		public ISelect<TIn, TOut> Get(ISelect<TIn, TOut> parameter)
			=> new Container(_aspect.Get(new Select(parameter)));

		internal sealed class Container : ISelect<TIn, TOut>
		{
			readonly ISelect<TI, TO> _select;

			public Container(ISelect<TI, TO> select) => _select = select;

			public TOut Get(TIn parameter) => (TOut)_select.Get(parameter);
		}

		sealed class Select : ISelect<TI, TO>
		{
			readonly ISelect<TIn, TOut> _select;

			public Select(ISelect<TIn, TOut> select) => _select = select;

			public TO Get(TI parameter) => _select.Get((TIn)parameter);
		}
	}

	sealed class AspectRegistrations<TIn, TOut> : IArray<Array<Type>, IAspect<TIn, TOut>>
	{
		public static AspectRegistrations<TIn, TOut> Default { get; } = new AspectRegistrations<TIn, TOut>();

		AspectRegistrations() : this(Leases<IAspect<TIn, TOut>>.Default, Implementations.Registrations,
		                             AdapterAspects<TIn, TOut>.Default) {}

		readonly IStores<IAspect<TIn, TOut>>         _stores;
		readonly Func<Array<IRegistration>>          _registrations;
		readonly ISelect<object, IAspect<TIn, TOut>> _adapter;

		public AspectRegistrations(IStores<IAspect<TIn, TOut>> stores, Func<Array<IRegistration>> registrations,
		                           ISelect<object, IAspect<TIn, TOut>> adapter)
		{
			_stores        = stores;
			_registrations = registrations;
			_adapter       = adapter;
		}

		public Array<IAspect<TIn, TOut>> Get(Array<Type> parameter)
		{
			var registrations = _registrations();
			var to            = registrations.Length;
			var elements      = _stores.Get(to);
			var source        = elements.Instance;
			var count         = 0u;

			for (var i = 0u; i < to; i++)
			{
				var registration = registrations[i];
				if (registration.Condition.Get(parameter))
				{
					var instance = registration.Get(parameter);
					source[count++] = instance is IAspect<TIn, TOut> aspect ? aspect : _adapter.Get(instance);
				}
			}

			return source.CopyInto(new IAspect<TIn, TOut>[count], 0, count);
		}
	}

	public sealed class AspectRegistry : SystemRegistry<IRegistration>
	{
		public static AspectRegistry Default { get; } = new AspectRegistry();

		AspectRegistry() {}
	}

	public interface IRegistration : IConditional<Array<Type>, object> {}

	public sealed class Registration : IRegistration
	{
		readonly Func<Array<Type>, Func<object>> _source;

		public Registration(Type definition) : this(Always<Array<Type>>.Default, definition) {}

		public Registration(ICondition<Array<Type>> condition, Type definition)
			: this(condition, AspectOpenGeneric.Default.Get(definition)) {}

		public Registration(ICondition<Array<Type>> condition, Func<Array<Type>, Func<object>> source)
		{
			_source   = source;
			Condition = condition;
		}

		public ICondition<Array<Type>> Condition { get; }

		public object Get(Array<Type> parameter) => _source(parameter)();
	}

	public sealed class AspectOpenGeneric : OpenGeneric
	{
		public static AspectOpenGeneric Default { get; } = new AspectOpenGeneric();

		AspectOpenGeneric() : base(typeof(IAspect<,>)) {}
	}

	public class Registration<TIn, TOut> : FixedResult<Array<Type>, object>, IRegistration
	{
		public Registration(IAspect<TIn, TOut> aspect) : this(Compare<TIn, TOut>.Default, aspect) {}

		public Registration(ICondition<Array<Type>> condition, IAspect<TIn, TOut> aspect) : base(aspect)
			=> Condition = condition;

		public ICondition<Array<Type>> Condition { get; }
	}

	sealed class Compare<TIn, TOut> : Condition<Array<Type>>
	{
		public static Compare<TIn, TOut> Default { get; } = new Compare<TIn, TOut>();

		Compare() : base(new Compare(A.Type<TIn>(), A.Type<TOut>())) {}
	}

	sealed class Compare : ICondition<Array<Type>>
	{
		readonly Array<ICondition<Type>> _conditions;
		readonly uint                    _length;

		public Compare(params Type[] types) : this(new Array<Type>(types)) {}

		public Compare(Array<Type> types)
			: this(types.Open().Select(x => new IsAssignableFrom(x)).ToArray(), types.Length) {}

		public Compare(Array<ICondition<Type>> conditions, uint length)
		{
			_conditions = conditions;
			_length     = length;
		}

		public bool Get(Array<Type> parameter)
		{
			if (parameter.Length == _length)
			{
				for (var i = 0u; i < _length; i++)
				{
					if (!_conditions[i].Get(parameter[i]))
					{
						return false;
					}
				}

				return true;
			}

			return false;
		}
	}

	public class CompositeAspect<TIn, TOut> : Aggregate<IAspect<TIn, TOut>, ISelect<TIn, TOut>>, IAspect<TIn, TOut>
	{
		public CompositeAspect(params IAspect<TIn, TOut>[] aspects) : this(new Array<IAspect<TIn, TOut>>(aspects)) {}

		public CompositeAspect(Array<IAspect<TIn, TOut>> items) : base(items) {}
	}

	public sealed class AssignedAspect<TIn, TOut> : ValidationAspect<TIn, TOut>
	{
		public static AssignedAspect<TIn, TOut> Default { get; } = new AssignedAspect<TIn, TOut>();

		AssignedAspect() : base(DefaultGuard<TIn>.Default.Execute) {}
	}

	public sealed class EmptyAspect<TIn, TOut> : Model.Selection.Select<ISelect<TIn, TOut>, ISelect<TIn, TOut>>, IAspect<TIn, TOut>
	{
		public static EmptyAspect<TIn, TOut> Default { get; } = new EmptyAspect<TIn, TOut>();

		EmptyAspect() : base(A.Self<ISelect<TIn, TOut>>()) {}
	}

	public class ValidationAspect<TIn, TOut> : Invocation0<ISelect<TIn, TOut>, Action<TIn>, ISelect<TIn, TOut>>,
	                                           IAspect<TIn, TOut>
	{
		public ValidationAspect(Action<TIn> parameter)
			: base((select, action) => new Configured<TIn, TOut>(select, action), parameter) {}
	}
}