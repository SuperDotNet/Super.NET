using Super.ExtensionMethods;
using Super.Model.Sources;
using Super.Runtime.Activation;
using System;
using Activator = Super.Runtime.Activation.Activator;

namespace Super.Runtime.Environment
{
	sealed class ComponentLocator<T> : DecoratedSource<Type, object>, IActivator
	{
		public static ComponentLocator<T> Default { get; } = new ComponentLocator<T>();

		ComponentLocator() : base(ComponentTypeLocator.Default.Out(Activator.Default.Assigned())) {}
	}
}