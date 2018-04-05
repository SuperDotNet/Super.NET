namespace Super.Model.Sources
{
	public interface IMutable<TParameter, TResult> : ISource<TParameter, TResult>, IAssignable<TParameter, TResult> {}
}
