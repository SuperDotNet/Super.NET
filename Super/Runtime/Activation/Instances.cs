using Super.Model.Sources;

namespace Super.Runtime.Activation
{
	sealed class Instances<TParameter, TResult> : Store<TParameter, TResult> where TResult : IActivateMarker<TParameter>
	{
		public static Instances<TParameter, TResult> Default { get; } = new Instances<TParameter, TResult>();

		Instances() {}
	}
}