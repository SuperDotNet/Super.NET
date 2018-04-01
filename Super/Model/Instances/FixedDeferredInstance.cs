using Super.ExtensionMethods;
using Super.Model.Sources;

namespace Super.Model.Instances
{
	public class FixedDeferredInstance<TParameter, TResult> : DeferredInstance<TResult>
	{
		public FixedDeferredInstance(ISource<TParameter, TResult> source, TParameter parameter)
			: base(source.Fix(parameter)) {}
	}
}