using Super.Model.Results;
using Super.Model.Selection;
using Super.Model.Selection.Alterations;
using Super.Model.Selection.Conditions;
using Super.Model.Sequences;
using Super.Reflection;
using Super.Reflection.Types;
using Super.Runtime;
using Super.Runtime.Environment;
using Super.Runtime.Invocation;
using Super.Runtime.Objects;
using System;
using System.Reflection;

namespace Super.Aspects
{
	public static class Extensions
	{
		public static ISelect<TIn, TOut> Configured<TIn, TOut>(this ISelect<TIn, TOut> @this)
			=> AspectConfiguration<TIn, TOut>.Default.Get(@this);

		public static IRegistration<TIn, TOut> Register<TIn, TOut>(this IAspect<TIn, TOut> @this)
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

	sealed class Aspect<TIn, TOut> : Select<ISelect<TIn, TOut>, ISelect<TIn, TOut>>, IAspect<TIn, TOut>
	{
		public static Aspect<TIn, TOut> Default { get; } = new Aspect<TIn, TOut>();

		Aspect() : base(x => x) {}
	}

	public interface IRegistration<TIn, TOut> : IConditional<ISelect<TIn, TOut>, IAspect<TIn, TOut>> {}

	class RuntimeRegistration<TIn, TOut> : Conditional<ISelect<TIn, TOut>, IAspect<TIn, TOut>>, IRegistration<TIn, TOut>
	{
		public RuntimeRegistration(Type definition) : this(new Objects(definition)) {}

		public RuntimeRegistration(IConditional<TypeInfo, object> conditional)
			: base(InstanceMetadata<ISelect<TIn, TOut>>.Default
			                                           .Select(conditional.Condition)
			                                           .ToCondition(),
			       InstanceMetadata<ISelect<TIn, TOut>>.Default
			                                           .Select(conditional)
			                                           .Then()
			                                           .Cast<IAspect<TIn, TOut>>()) {}
	}

	class Objects : Conditional<TypeInfo, object>
	{
		public Objects(Type definition)
			: base(ImplementsSelection.Default,
			       GenericInterfaceImplementations.Default.Select(new RuntimeSelection(definition))
			                                      .Then()
			                                      .Value()) {}
	}

	public sealed class ImplementsSelection : ImplementsGenericType
	{
		public static ImplementsSelection Default { get; } = new ImplementsSelection();

		ImplementsSelection() : base(typeof(ISelect<,>)) {}
	}

	sealed class RuntimeSelection : ISelect<IConditional<Type, Array<TypeInfo>>, IResult<object>>
	{
		readonly IGeneric<object> _generic;
		readonly Type             _parameter;

		public RuntimeSelection(Type definition) : this(definition, typeof(ISelect<,>)) {}

		public RuntimeSelection(Type definition, Type parameter) : this(new Generic<object>(definition), parameter) {}

		public RuntimeSelection(IGeneric<object> generic, Type parameter)
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
	}

	public class Registration<TIn, TOut> : Conditional<ISelect<TIn, TOut>, IAspect<TIn, TOut>>, IRegistration<TIn, TOut>
	{
		public Registration(IAspect<TIn, TOut> aspect)
			: base(Always<ISelect<TIn, TOut>>.Default, aspect.Start(I.A<ISelect<TIn, TOut>>())) {}
	}

	public sealed class Selector<TIn, TOut> : IArray<ISelect<TIn, TOut>, IAspect<TIn, TOut>>
	{
		readonly Array<IRegistration<TIn, TOut>> _registrations;
		readonly IStores<IAspect<TIn, TOut>>     _stores;

		public Selector(Array<IRegistration<TIn, TOut>> registrations)
			: this(registrations, Leases<IAspect<TIn, TOut>>.Default) {}

		public Selector(Array<IRegistration<TIn, TOut>> registrations, IStores<IAspect<TIn, TOut>> stores)
		{
			_registrations = registrations;
			_stores        = stores;
		}

		public Array<IAspect<TIn, TOut>> Get(ISelect<TIn, TOut> parameter)
		{
			var to       = _registrations.Length;
			var elements = _stores.Get(to);
			var count    = 0u;
			var source   = elements.Instance;

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
	}

	public sealed class AspectRegistry<TIn, TOut> : Registry<IAspect<TIn, TOut>>
	{
		public static AspectRegistry<TIn, TOut> Default { get; } = new AspectRegistry<TIn, TOut>();

		AspectRegistry() : this(AssignedAspect<TIn, TOut>.Default) {}

		public AspectRegistry(params IAspect<TIn, TOut>[] elements) : base(elements) {}
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

	public class ValidationAspect<TIn, TOut> : Invocation0<ISelect<TIn, TOut>, Action<TIn>, ISelect<TIn, TOut>>,
	                                           IAspect<TIn, TOut>
	{
		public ValidationAspect(Action<TIn> parameter)
			: base((select, action) => new Configured<TIn, TOut>(select, action), parameter) {}
	}
}