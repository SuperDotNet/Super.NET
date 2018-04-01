using Super.Model.Instances;

namespace Super.Runtime.Activation
{
	sealed class ActivatorInstance<T> : IInstance<object> where T : class
	{
		public static ActivatorInstance<T> Default { get; } = new ActivatorInstance<T>();

		ActivatorInstance() : this(Activator<T>.Default) {}

		readonly IActivator<T> _activator;

		public ActivatorInstance(IActivator<T> activator) => _activator = activator;

		public object Get() => _activator.Get();
	}
}