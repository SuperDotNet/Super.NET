using Super.ExtensionMethods;
using Super.Model.Containers;
using Super.Model.Instances;
using Super.Reflection;
using Super.Runtime.Activation;
using System;

namespace Super.Runtime.Environment
{
	class Component<T> : FixedDeferredSingleton<Type, T>
	{
		public Component() : this(ComponentLocator<T>.Default) {}

		public Component(IActivator activator) : this(activator, Default<T>.Instance) {}

		public Component(T fallback) : this(fallback.ToInstance()) {}

		public Component(IInstance<T> fallback) : this(ComponentLocator<T>.Default, fallback) {}

		public Component(IActivator activator, IInstance<T> fallback)
			: base(activator.Out(Container<T>.Default).Or(fallback).Guard(LocateMessage.Default).Protect(),
			       Types<T>.Identity) {}
	}
}