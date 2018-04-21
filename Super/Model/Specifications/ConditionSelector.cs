using Super.Model.Selection;

namespace Super.Model.Specifications
{
	public sealed class ConditionSelector : Select<ISpecification, bool>
	{
		public static ConditionSelector Default { get; } = new ConditionSelector();

		ConditionSelector() : base(x => x.IsSatisfiedBy()) {}
	}
}
