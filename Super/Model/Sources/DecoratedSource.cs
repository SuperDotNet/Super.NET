using Super.ExtensionMethods;

namespace Super.Model.Sources
{
	public class DecoratedSource<TParameter, TResult> : DelegatedSource<TParameter, TResult>
	{
		public DecoratedSource(ISource<TParameter, TResult> source) : base(source.ToDelegate()) {}
	}
}