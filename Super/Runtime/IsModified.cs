using JetBrains.Annotations;
using Super.Compose;
using Super.Model.Selection;
using Super.Model.Selection.Conditions;
using Super.Model.Selection.Stores;
using Super.Reflection;
using Super.Reflection.Types;
using Super.Runtime.Invocation;
using System;
using System.Reflection;

namespace Super.Runtime
{
	sealed class IsModified<T> : InverseCondition<T>
	{
		public static IsModified<T> Default { get; } = new IsModified<T>();

		IsModified() : base(IsDefault<T>.Default) {}
	}

	sealed class IsAssigned : InverseCondition<object>
	{
		public static IsAssigned Default { get; } = new IsAssigned();

		IsAssigned() : base(IsNullReference.Default) {}
	}

	sealed class HasValue<T> : Condition<T?> where T : struct
	{
		[UsedImplicitly]
		public static HasValue<T> Default { get; } = new HasValue<T>();

		HasValue() : base(x => x.HasValue) {}
	}

	sealed class IsAssignedConditions<T> : ReferenceValueStore<TypeInfo, Func<T, bool>>
	{
		[UsedImplicitly]
		public static IsAssignedConditions<T> Default { get; } = new IsAssignedConditions<T>();

		IsAssignedConditions() : base(Start.A.Selection.Of.System.Metadata.By.Returning(IsModified<T>.Default)
		                                   .Unless(IsReference.Default,
		                                           Start.A.Selection<T>()
		                                                .AndOf<object>()
		                                                .By.Cast.Select(IsAssigned.Default)
		                                                .Start())
		                                   .Unless(IsAssignableStructure.Default,
		                                           Start.A.Generic(typeof(HasValue<>))
		                                                .Of.Type<T>()
		                                                .As.Condition()
		                                                .Then()
		                                                .Invoke()
		                                                .Get()
		                                                .In(Type<T>.Instance.Yield().Result()))
		                                   .To(AccountForUnassignedType.Default.Select)
		                                   .Select(x => x.ToDelegate())
		                                   .Get) {}
	}

	sealed class IsAssigned<T> : Select<T, bool>, ICondition<T>
	{
		public static IsAssigned<T> Default { get; } = new IsAssigned<T>();

		IsAssigned() : base(IsAssignedConditions<T>.Default.Get(typeof(T))) {}
	}

	sealed class IsNullReference : Condition<object>
	{
		public static IsNullReference Default { get; } = new IsNullReference();

		IsNullReference() : base(EqualsNullReference.Default.Get) {}
	}

	sealed class EqualsNullReference : Invocation0<object, object, bool>
	{
		public static EqualsNullReference Default { get; } = new EqualsNullReference();

		EqualsNullReference() : base(ReferenceEquals, null) {}
	}

	public class IsAssigned<TIn, TOut> : Condition<TIn> where TOut : class
	{
		protected IsAssigned(Func<TIn, TOut> @select) : this(@select.Start()) {}

		protected IsAssigned(ISelect<TIn, TOut> source) : base(source.Select(IsAssigned.Default)) {}
	}
}