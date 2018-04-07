using Super.Model.Instances;

namespace Super.Runtime.Activation
{
	sealed class ActivatorInstance<T> : DecoratedInstance<object> where T : class
	{
		public static ActivatorInstance<T> Default { get; } = new ActivatorInstance<T>();

		ActivatorInstance() : base(Activator<T>.Default) {}
	}
}