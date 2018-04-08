using Super.Model.Selection;
using Super.Model.Selection.Stores;

namespace Super.Runtime.Activation
{
	sealed class Activations<TParameter, TResult> : Decorated<TParameter, TResult> where TResult : IActivateMarker<TParameter>
	{
		public static Activations<TParameter, TResult> Default { get; } = new Activations<TParameter, TResult>();

		Activations() : base(Instances<TParameter, TResult>.Default) {}
	}

	sealed class Instances<TParameter, TResult> : Store<TParameter, TResult>
	{
		public static Instances<TParameter, TResult> Default { get; } = new Instances<TParameter, TResult>();

		Instances() {}
	}
}