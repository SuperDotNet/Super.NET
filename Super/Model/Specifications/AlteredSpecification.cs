using Super.Model.Sources;

namespace Super.Model.Specifications
{
	public sealed class AlteredSpecification<T> : SelectedParameterSpecification<T, T>
	{
		public AlteredSpecification(ISpecification<T> specification, ISource<T, T> alteration) : base(specification, alteration) {}
	}
}