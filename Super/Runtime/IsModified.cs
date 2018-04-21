using Super.Model.Selection;
using Super.Model.Specifications;
using Super.Reflection;
using Super.Reflection.Types;
using Super.Runtime.Invocation;
using System;

namespace Super.Runtime
{
	/*sealed class IsModified<T> : InverseSpecification<T>
	{
		public static IsModified<T> Default { get; } = new IsModified<T>();

		IsModified() : base(IsDefault<T>.Default) {}
	}*/

	sealed class IsAssigned : InverseSpecification<object>
	{
		public static IsAssigned Default { get; } = new IsAssigned();

		IsAssigned() : base(IsNullReference.Default) {}
	}

	sealed class IsAssigned<T> : DelegatedSpecification<T>
	{
		readonly static Func<T, bool> Always = Always<T>.Default.IsSatisfiedBy;

		public static IsAssigned<T> Default { get; } = new IsAssigned<T>();

		IsAssigned() : base(CanBeAssigned.Default.IsSatisfiedBy(Type<T>.Instance)
			                    ? new Parameter<object, T, bool>(IsAssigned.Default.IsSatisfiedBy, arg => arg)
				                    .Get
			                    : Always) {}
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
			: base(new Result<TParameter, TResult, bool>(source, IsAssigned.Default.IsSatisfiedBy).Get) {}
	}

	/*public class IsModified<TParameter, TResult> : DelegatedSpecification<TParameter>
	{
		readonly static Func<TResult, bool> Assigned = IsModified<TResult>.Default.IsSatisfiedBy;

		protected IsModified(Func<TParameter, TResult> source) : this(source, Assigned) {}

		IsModified(Func<TParameter, TResult> source, Func<TResult, bool> result)
			: base(new Parameter<TResult, TParameter, bool>(result, source).Get) {}
	}*/
}