using Super.Model.Selection;
using Super.Model.Sources;
using Super.Reflection.Types;
using Super.Runtime.Activation;
using System;

namespace Super.Runtime.Environment
{
	class Component<T> : FixedDeferredSingleton<Type, T>
	{
		public Component() : this(ComponentLocator<T>.Default) {}

		public Component(IActivator activator) : this(activator, Model.Sources.Default<T>.Instance) {}

		public Component(T fallback) : this(fallback.ToSource()) {}

		public Component(ISource<T> fallback) : this(ComponentLocator<T>.Default, fallback) {}

		public Component(IActivator activator, ISource<T> fallback)
			: base(activator.Out(CastOrValue<T>.Default).Or(fallback).Guard(LocateMessage.Default).Protect(),
			       Type<T>.Instance) {}
	}
}