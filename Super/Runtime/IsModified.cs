using Super.Model.Selection;
using Super.Model.Specifications;
using System;

namespace Super.Runtime
{
	sealed class IsModified<T> : InverseSpecification<T>
	{
		public static IsModified<T> Default { get; } = new IsModified<T>();

		IsModified() : base(IsDefault<T>.Default) {}
	}

	public class IsModified<TParameter, TResult> : DelegatedSpecification<TParameter>
	{
		readonly static Func<TResult, bool> Assigned = IsModified<TResult>.Default.IsSatisfiedBy;

		protected IsModified(Func<TParameter, TResult> source) : this(source, Assigned) {}

		IsModified(Func<TParameter, TResult> source, Func<TResult, bool> result)
			: base(new Parameter<TResult, TParameter, bool>(result, source).Get) {}
	}
}