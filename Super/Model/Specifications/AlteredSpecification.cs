using Super.Model.Selection;

namespace Super.Model.Specifications
{
	public sealed class AlteredSpecification<T> : SelectedParameterSpecification<T, T>
	{
		public AlteredSpecification(ISpecification<T> specification, ISelect<T, T> alteration) : base(specification, alteration) {}
	}
}