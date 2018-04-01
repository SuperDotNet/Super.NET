using Super.ExtensionMethods;
using Super.Model.Instances;
using Super.Model.Sources;
using Super.Reflection;
using System;

namespace Super.Runtime.Environment
{
	class Ambient<T> : FixedDeferredInstance<Type, T>
	{
		public Ambient() : this(Locator<T>.Default) {}

		public Ambient(ISource<Type, object> instance) : this(instance, Model.Instances.Default<T>.Instance) {}

		public Ambient(T fallback) : this(fallback.ToInstance()) {}

		public Ambient(IInstance<T> fallback) : this(Locator<T>.Default, fallback) {}

		public Ambient(ISource<Type, object> instance, IInstance<T> fallback)
			: base(instance.OutOrInstance(I<T>.Default).Or(fallback.Allow()).Guard(LocateMessage.Default), Types<T>.Identity) {}
	}
}