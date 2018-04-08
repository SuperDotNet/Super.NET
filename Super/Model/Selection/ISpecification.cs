using Super.Model.Specifications;

namespace Super.Model.Selection
{
	public interface
		ISpecification<in TParameter, out TResult> : ISpecification<TParameter>, ISelect<TParameter, TResult> {}
}