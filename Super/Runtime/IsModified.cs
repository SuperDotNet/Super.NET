using Super.Model.Selection;
using Super.Model.Specifications;
using Super.Reflection;
using Super.Reflection.Types;
using Super.Runtime.Invocation;
using System;

namespace Super.Runtime
{
	sealed class IsModified<T> : InverseSpecification<T>
	{
		public static IsModified<T> Default { get; } = new IsModified<T>();

		IsModified() : base(IsDefault<T>.Default) {}
	}

	sealed class IsAssigned : InverseSpecification<object>
	{
		public static IsAssigned Default { get; } = new IsAssigned();

		IsAssigned() : base(IsNullReference.Default) {}
	}

	sealed class HasValue<T> : DelegatedSpecification<T?> where T : struct
	{
		public static HasValue<T> Default { get; } = new HasValue<T>();

		HasValue() : base(x => x.HasValue) {}
	}

	sealed class HasAssignment<T> : DecoratedSpecification<T> where T : class
	{
		public static HasAssignment<T> Default { get; } = new HasAssignment<T>();

		HasAssignment() : base(IsAssigned.Default) {}
	}

	sealed class IsAssigned<T> : DelegatedSpecification<T>
	{
		readonly static Func<T, bool> Value = IsModified<T>.Default.IsSatisfiedBy;

		public static IsAssigned<T> Default { get; } = new IsAssigned<T>();

		IsAssigned()
			: base(IsReference.Default.IsSatisfiedBy(Type<T>.Instance)
				       ? new Generic<ISpecification<T>>(typeof(HasAssignment<>)).Get(Type<T>.Instance)
				                                                                .Invoke()
				                                                                .IsSatisfiedBy
				       : IsAssignableStructure.Default.IsSatisfiedBy(Type<T>.Instance)
					       ? new Generic<ISpecification<T>>(typeof(HasValue<>))
					         .Get(AccountForUnassignedAlteration.Default.Get(Type<T>.Instance))
					         .Invoke()
					         .IsSatisfiedBy
					       : Value) {}
	}

	sealed class IsNullReference : DelegatedSpecification<object>
	{
		public static IsNullReference Default { get; } = new IsNullReference();

		IsNullReference() : base(EqualsNullReference.Default.Get) {}
	}

	sealed class EqualsNullReference : Invocation0<object, object, bool>
	{
		public static EqualsNullReference Default { get; } = new EqualsNullReference();

		EqualsNullReference() : base(ReferenceEquals, null) {}
	}

	public class IsAssigned<TParameter, TResult> : DelegatedSpecification<TParameter> where TResult : class
	{
		protected IsAssigned(Func<TParameter, TResult> source)
			: base(new Selection<TParameter, TResult, bool>(source, IsAssigned.Default.IsSatisfiedBy).Get) {}
	}
}