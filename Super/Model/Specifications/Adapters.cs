using Super.Model.Sources;

namespace Super.Model.Specifications
{
	sealed class Adapters<T> : ReferenceStore<ISpecification<T>, SpecificationSourceAdapter<T>>
	{
		public static Adapters<T> Default { get; } = new Adapters<T>();

		Adapters() : base(x => new SpecificationSourceAdapter<T>(x)) {}
	}
}