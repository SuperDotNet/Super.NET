using Super.Model.Selection.Stores;
using System;

namespace Super.Model.Specifications
{
	sealed class Delegates<T> : ReferenceStore<ISpecification<T>, Func<T, bool>>
	{
		public static Delegates<T> Default { get; } = new Delegates<T>();

		Delegates() : base(x => x.IsSatisfiedBy) {}
	}
}
