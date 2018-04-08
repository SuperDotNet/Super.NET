using Super.Model.Sources;

namespace Super.Runtime.Activation
{
	sealed class ActivatorSource<T> : Decorated<object> where T : class
	{
		public static ActivatorSource<T> Default { get; } = new ActivatorSource<T>();

		ActivatorSource() : base(Activator<T>.Default) {}
	}
}