using Super.Model.Specifications;

namespace Super.Model.Sources
{
	public interface
		ISpecification<in TParameter, out TResult> : ISpecification<TParameter>, ISource<TParameter, TResult> {}
}