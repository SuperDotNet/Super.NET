using Super.Model.Selection;
using Super.Model.Selection.Stores;

namespace Super.Runtime.Activation
{
	public sealed class MarkedActivations<TParameter, TResult> : DecoratedSelect<TParameter, TResult> where TResult : IActivateMarker<TParameter>
	{
		public static MarkedActivations<TParameter, TResult> Default { get; } = new MarkedActivations<TParameter, TResult>();

		MarkedActivations() : base(Activations<TParameter, TResult>.Default) {}
	}

	sealed class Activations<TParameter, TResult> : Store<TParameter, TResult>
	{
		public static Activations<TParameter, TResult> Default { get; } = new Activations<TParameter, TResult>();

		Activations() {}
	}
}