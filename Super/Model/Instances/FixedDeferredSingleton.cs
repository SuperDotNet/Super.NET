using Super.ExtensionMethods;
using Super.Model.Sources;

namespace Super.Model.Instances
{
	public class FixedDeferredSingleton<TParameter, TResult> : DeferredSingleton<TResult>
	{
		public FixedDeferredSingleton(ISource<TParameter, TResult> source, TParameter parameter) :
			base(source.Select(parameter)) {}
	}
}