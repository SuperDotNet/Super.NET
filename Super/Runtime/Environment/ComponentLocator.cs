using Super.Reflection;
using Super.Runtime.Activation;
using Activator = Super.Runtime.Activation.Activator;

namespace Super.Runtime.Environment
{
	sealed class ComponentLocator<T> : FixedActivator<T>
	{
		public static ComponentLocator<T> Default { get; } = new ComponentLocator<T>();

		ComponentLocator() : base(ComponentTypeLocator.Default
		                                              .Select(Activator.Default.Assigned())
		                                              .CastForValue(I<T>.Default)) {}
	}
}