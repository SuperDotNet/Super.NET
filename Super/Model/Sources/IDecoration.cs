namespace Super.Model.Sources
{
	public interface IDecoration<TParameter, TResult> : ISource<Decoration<TParameter, TResult>, TResult> {}
}