using Super.Model.Selection;

namespace Super.Model.Sources
{
	public class FixedDeferredSingleton<TParameter, TResult> : DeferredSingleton<TResult>
	{
		public FixedDeferredSingleton(ISelect<TParameter, TResult> @select, TParameter parameter)
			: base(new FixedSelection<TParameter,TResult>(@select, parameter)) {}
	}
}