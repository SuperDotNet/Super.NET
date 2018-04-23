using Super.Model.Selection;
using System;

namespace Super.Model.Specifications
{
	sealed class DelegateSelector<T> : ISelect<ISpecification<T>, Func<T, bool>>
	{
		public static DelegateSelector<T> Default { get; } = new DelegateSelector<T>();

		DelegateSelector() {}

		public Func<T, bool> Get(ISpecification<T> parameter) => parameter.IsSatisfiedBy;
	}
}
