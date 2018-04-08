using Super.Model.Selection.Stores;

namespace Super.Model.Specifications
{
	sealed class InverseSpecifications<T> : ReferenceStore<ISpecification<T>, InverseSpecification<T>>
	{
		public static InverseSpecifications<T> Default { get; } = new InverseSpecifications<T>();

		InverseSpecifications() : base(x => new InverseSpecification<T>(x)) {}
	}
}