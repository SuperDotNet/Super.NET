using Super.Runtime.Activation;

namespace Super.Model.Results
{
	public class DecoratedResult<T> : DelegatedResult<T>, IActivateUsing<IResult<T>>
	{
		public DecoratedResult(IResult<T> source) : base(source.Get) {}
	}
}