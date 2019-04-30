using Super.Compose;
using Super.Model.Selection;
using Super.Model.Selection.Alterations;
using Super.Model.Selection.Conditions;
using Super.Model.Sequences;
using Super.Reflection.Types;
using Super.Runtime;
using Super.Runtime.Environment;
using Super.Runtime.Invocation;
using System;
using System.Linq;
using System.Reflection;

namespace Super.Aspects
{
	public static class Extensions
	{
		public static ISelect<TIn, TOut> Configured<TIn, TOut>(this ISelect<TIn, TOut> @this)
			=> AspectConfiguration<TIn, TOut>.Default.Get(@this);

		public static IRegistration Register<TIn, TOut>(this IAspect<TIn, TOut> @this)
			=> new Registration<TIn, TOut>(@this);
	}

	public sealed class AspectConfiguration<TIn, TOut> : SystemStore<IAspect<TIn, TOut>>, IAspect<TIn, TOut>
	{
		public static AspectConfiguration<TIn, TOut> Default { get; } = new AspectConfiguration<TIn, TOut>();

		AspectConfiguration() : base(Apply<TIn, TOut>.Default.Start()) {}

		public ISelect<TIn, TOut> Get(ISelect<TIn, TOut> parameter) => Get().Get(parameter);
	}

	/*public sealed class Runtime<TIn, TOut> : IAspect<TIn, TOut>
	{
		readonly ISelect<TypeInfo, Array<object>> _select;

		public Runtime() : this(null) {}

		public Runtime(ISelect<TypeInfo, Array<object>> select)
		{
			_select = @select;
		}

		public ISelect<TIn, TOut> Get(ISelect<TIn, TOut> parameter) => ;
	}*/

	public interface IAspect<TIn, TOut> : IAlteration<ISelect<TIn, TOut>> {}

	/*sealed class Aspect<TIn, TOut> : Select<ISelect<TIn, TOut>, ISelect<TIn, TOut>>, IAspect<TIn, TOut>
	{
		public static Aspect<TIn, TOut> Default { get; } = new Aspect<TIn, TOut>();

		Aspect() : base(x => x) {}
	}*/

	/*sealed class AspectRegistration : ICommand<Type>
	{
		public static AspectRegistration Default { get; } = new AspectRegistration();

		AspectRegistration() {}

		public void Execute(Type parameter) {}

		public void Execute<TIn, TOut>(IAspect<TIn, TOut> instance) {}
	}*/

	public sealed class Aspects<TIn, TOut> : Select<ISelect<TIn, TOut>, IAspect<TIn, TOut>>
	{
		public static Aspects<TIn, TOut> Default { get; } = new Aspects<TIn, TOut>();

		Aspects() : base(Start.A.Selection<ISelect<TIn, TOut>>()
		                      .By.Type.Then()
		                      .Select(RegisteredAspects<TIn, TOut>.Default.ToStore())) {}
		/*public Aspects(ISelect<ISelect<TIn, TOut>, IAspect<TIn, TOut>> @select) : base(@select, Stores.New<IAspect<TIn, TOut>>()) {}

		public Aspects(ISelect<ISelect<TIn, TOut>, IAspect<TIn, TOut>> @select,
		               ITable<ISelect<TIn, TOut>, IAspect<TIn, TOut>> assignable) : base(@select, assignable) {}*/
	}

	sealed class RegisteredAspects<TIn, TOut> : ISelect<Type, IAspect<TIn, TOut>>
	{
		public static RegisteredAspects<TIn, TOut> Default { get; } = new RegisteredAspects<TIn, TOut>();

		RegisteredAspects() : this(AspectRegistrations<TIn, TOut>.Default) {}

		readonly ISelect<Type, IConditional<Type, Array<TypeInfo>>> _implementations;
		readonly IArray<Array<Type>, IAspect<TIn, TOut>>            _registrations;
		readonly IAspect<TIn, TOut>                                 _empty;

		public RegisteredAspects(IArray<Array<Type>, IAspect<TIn, TOut>> registrations)
			: this(GenericInterfaceImplementations.Default, registrations, EmptyAspect<TIn, TOut>.Default) {}

		public RegisteredAspects(ISelect<Type, IConditional<Type, Array<TypeInfo>>> implementations,
		                         IArray<Array<Type>, IAspect<TIn, TOut>> registrations, IAspect<TIn, TOut> empty)
		{
			_implementations = implementations;
			_registrations   = registrations;
			_empty           = empty;
		}

		public IAspect<TIn, TOut> Get(Type parameter)
		{
			var implementations = _implementations.Get(parameter).Get(typeof(ISelect<,>));

			var length = implementations.Length;
			for (var i = 0u; i < length; i++)
			{
				var registrations = _registrations.Get(implementations[i].GenericTypeParameters);
				if (registrations.Length > 0)
				{
					return new CompositeAspect<TIn, TOut>(registrations);
				}
			}

			return _empty;
		}
	}

	public sealed class AspectRegistrations<TIn, TOut> : IArray<Array<Type>, IAspect<TIn, TOut>>
	{
		public static AspectRegistrations<TIn, TOut> Default { get; } = new AspectRegistrations<TIn, TOut>();

		AspectRegistrations() : this(Leases<IAspect<TIn, TOut>>.Default, AspectRegistry.Default.Get) {}

		readonly IStores<IAspect<TIn, TOut>> _stores;
		readonly Func<Array<IRegistration>>  _registrations;

		public AspectRegistrations(IStores<IAspect<TIn, TOut>> stores, Func<Array<IRegistration>> registrations)
		{
			_stores        = stores;
			_registrations = registrations;
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
					source[count++] = registration.Get(parameter).To<IAspect<TIn, TOut>>();
				}
			}

			var result = source.CopyInto(new IAspect<TIn, TOut>[count], 0, count);
			return result;
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

	/*sealed class AspectTypeRegistry : Registry<Func<Array<Type>, object>>
	{
		public AspectTypeRegistry() : this(new ContainsGenericInterfaceGuard(typeof(IAspect<,>)),
		                                   Array<Type>.Empty.Start().Variable()) {}

		public AspectTypeRegistry(ICommand<Type> guard, IMutable<Array<Type>> source)
			: base(source,
			       guard.Then().Many().ToConfiguration().Terminate(new AddRange<Type>(source)).Get(),
			       guard.Then().ToConfiguration().Terminate(new Add<Type>(source)).Get()) {}
	}*/

	/*public interface IRegistration<TIn, TOut> : IConditional<ISelect<TIn, TOut>, IAspect<TIn, TOut>> {}*/

	public class Registration<TIn, TOut> : FixedResult<Array<Type>, object>, IRegistration //<TIn, TOut>
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

		public Compare(params Type[] types) : this(new Array<Type>(types)) {}

		public Compare(Array<Type> conditions)
			: this(conditions.Open().Select(x => new IsAssignableFrom(x)).ToArray()) {}

		public Compare(Array<ICondition<Type>> conditions) => _conditions = conditions;

		public bool Get(Array<Type> parameter)
		{
			var length = _conditions.Length;
			if (parameter.Length == length)
			{
				for (var i = 0u; i < length; i++)
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

	/*sealed class RuntimeRegistration<TIn, TOut> : Conditional<ISelect<TIn, TOut>, IAspect<TIn, TOut>>,
	                                              IRegistration<TIn, TOut>
	{
		public RuntimeRegistration(Type definition)
			: this(InstanceMetadata<ISelect<TIn, TOut>>.Default, new Instances(definition)) {}

		public RuntimeRegistration(ISelect<ISelect<TIn, TOut>, TypeInfo> metadata,
		                           IConditional<TypeInfo, object> conditional)
			: base(metadata.Select(conditional.Condition).Then(),
			       metadata.Select(conditional).Then().Cast<IAspect<TIn, TOut>>()) {}
	}*/

	/*sealed class Instances : Conditional<TypeInfo, object>
	{
		public Instances(Type definition)
			: base(ImplementsSelection.Default,
			       GenericInterfaceImplementations.Default.Select(new Instance(definition))
			                                      .Then()
			                                      .Value()) {}
	}

	sealed class Instance : ISelect<IConditional<Type, Array<TypeInfo>>, IResult<object>>
	{
		readonly IGeneric<object> _generic;
		readonly Type             _parameter;

		public Instance(Type definition) : this(definition, typeof(ISelect<,>)) {}

		public Instance(Type definition, Type parameter) : this(new Generic<object>(definition), parameter) {}

		public Instance(IGeneric<object> generic, Type parameter)
		{
			_generic   = generic;
			_parameter = parameter;
		}

		public IResult<object> Get(IConditional<Type, Array<TypeInfo>> parameter)
			=> parameter.In(_parameter)
			            .Query()
			            .Select(GenericArguments.Default)
			            .Select(_generic)
			            .First()
			            .Then()
			            .Invoke()
			            .Get()
			            .Out();
	}*/

	/*public sealed class Selector<TIn, TOut> : IArray<ISelect<TIn, TOut>, IAspect<TIn, TOut>>
	{
		readonly Array<IRegistration>        _registrations;
		readonly IStores<IAspect<TIn, TOut>> _stores;

		public Selector(Func<Array<IRegistration>> registrations)
			: this(registrations, Leases<IAspect<TIn, TOut>>.Default) {}

		public Selector(Func<Array<IRegistration>> registrations, IStores<IAspect<TIn, TOut>> stores)
		{
			_registrations = registrations;
			_stores        = stores;
		}

		public Array<IAspect<TIn, TOut>> Get(ISelect<TIn, TOut> parameter)
		{
			var to       = _registrations.Length;
			var elements = _stores.Get(to);
			var source   = elements.Instance;
			var count    = 0u;

			for (var i = 0u; i < to; i++)
			{
				var registration = _registrations[i];
				if (registration.Condition.Get(parameter))
				{
					source[count++] = registration.Get(parameter);
				}
			}

			var result = source.CopyInto(new IAspect<TIn, TOut>[count], 0, count);
			return result;
		}
	}*/

	public sealed class AspectRegistry<TIn, TOut> : Registry<IAspect<TIn, TOut>>
	{
		public static AspectRegistry<TIn, TOut> Default { get; } = new AspectRegistry<TIn, TOut>();

		AspectRegistry() : this(AssignedAspect<TIn, TOut>.Default) {}

		public AspectRegistry(params IAspect<TIn, TOut>[] elements) : base(elements) {}
	}

	public class CompositeAspect<TIn, TOut> : Aggregate<IAspect<TIn, TOut>, ISelect<TIn, TOut>>, IAspect<TIn, TOut>
	{
		public CompositeAspect(Array<IAspect<TIn, TOut>> items) : base(items) {}
	}

	public sealed class Apply<TIn, TOut> : Aggregate<IAspect<TIn, TOut>, ISelect<TIn, TOut>>, IAspect<TIn, TOut>
	{
		public static Apply<TIn, TOut> Default { get; } = new Apply<TIn, TOut>();

		Apply() : base(AspectRegistry<TIn, TOut>.Default) {}
	}

	public sealed class AssignedAspect<TIn, TOut> : ValidationAspect<TIn, TOut>
	{
		public static AssignedAspect<TIn, TOut> Default { get; } = new AssignedAspect<TIn, TOut>();

		AssignedAspect() : base(DefaultGuard<TIn>.Default.Execute) {}
	}

	public sealed class EmptyAspect<TIn, TOut> : Select<ISelect<TIn, TOut>, ISelect<TIn, TOut>>, IAspect<TIn, TOut>
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