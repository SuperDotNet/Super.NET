using System;
using Super.ExtensionMethods;
using Super.Model.Instances;
using Super.Model.Sources;
using Super.Reflection;

namespace Super.Runtime.Environment
{
	class Component<T> : FixedDeferredInstance<Type, T>
	{
		public Component() : this(Locator<T>.Default) {}

		public Component(ISource<Type, object> instance) : this(instance, Model.Instances.Default<T>.Instance) {}

		public Component(IInstance<T> fallback) : this(Locator<T>.Default, fallback) {}

		public Component(ISource<Type, object> instance, IInstance<T> fallback)
			: base(instance.OutOrInstance(I<T>.Default).Or(fallback.Allow()).Guard(LocateMessage.Default), Types<T>.Identity) {}
	}
}