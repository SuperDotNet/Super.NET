using Super.ExtensionMethods;
using Super.Model.Sources;
using System;
using Activator = Super.Runtime.Activation.Activator;

namespace Super.Runtime.Environment
{
	sealed class Locator<T> : DecoratedSource<Type, object>
	{
		public static Locator<T> Default { get; } = new Locator<T>();

		Locator() : base(ComponentTypeLocator.Default.Assigned(Activator.Default)) {}
	}
}