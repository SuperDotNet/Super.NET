﻿using Super.ExtensionMethods;
using Super.Model.Instances;
using Super.Reflection;

namespace Super.Runtime.Activation
{
	sealed class Singleton<T> : DeferredInstance<T>, ISingleton<T>
	{
		public static Singleton<T> Default { get; } = new Singleton<T>();

		Singleton() : base(Singletons.Default.Fix(Types<T>.Identity).Adapt().Out(I<T>.Default).Get) {}
	}
}