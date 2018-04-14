﻿using Super.ExtensionMethods;
using Super.Model.Selection;
using Super.Model.Sources;
using Super.Reflection;

namespace Super.Runtime.Activation
{
	sealed class Singleton<T> : DeferredSingleton<T>, ISingleton<T>
	{
		public static Singleton<T> Default { get; } = new Singleton<T>();

		Singleton() : base(Singletons.Default.Select(Type<T>.Instance).Select(Cast<T>.Default).Get) {}
	}
}