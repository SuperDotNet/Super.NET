using Super.ExtensionMethods;
using Super.Model.Instances;

namespace Super.Runtime.Activation
{
	public sealed class Activator<T> : DecoratedInstance<T>, IActivator<T>
	{
		public static Activator<T> Default { get; } = new Activator<T>();

		Activator() : base(Singleton<T>.Default.Adapt().Or(New<T>.Default.Adapt())) {}
	}
}