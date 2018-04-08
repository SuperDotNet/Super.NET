using Super.Model.Selection.Stores;
using System;

namespace Super.Model.Specifications
{
	sealed class Specifications<T> : ReferenceStore<Func<T, bool>, ISpecification<T>>
	{
		public static Specifications<T> Default { get; } = new Specifications<T>();

		Specifications() : base(x => x.Target as ISpecification<T> ?? new DelegatedSpecification<T>(x)) {}
	}
}