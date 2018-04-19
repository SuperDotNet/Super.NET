using Super.Runtime.Activation;
using System;
using Super.Model.Selection;
using Activator = Super.Runtime.Activation.Activator;

namespace Super.Runtime.Environment
{
	sealed class ComponentLocator<T> : DecoratedSelect<Type, object>, IActivator
	{
		public static ComponentLocator<T> Default { get; } = new ComponentLocator<T>();

		ComponentLocator() : base(ComponentTypeLocator.Default.Out(Activator.Default.Assigned())) {}
	}
}