using Super.ExtensionMethods;
using Super.Model.Sources;

namespace Super.Model.Specifications
{
	sealed class SpecificationSourceCoercer<T> : ISource<ISpecification<T>, ISource<T, bool>>
	{
		public static SpecificationSourceCoercer<T> Default { get; } = new SpecificationSourceCoercer<T>();

		SpecificationSourceCoercer() {}

		public ISource<T, bool> Get(ISpecification<T> parameter) => parameter.Adapt();
	}
}