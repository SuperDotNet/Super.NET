using Super.ExtensionMethods;
using Super.Model.Collections;
using Super.Model.Selection;
using Super.Model.Sources;
using Super.Reflection;
using Super.Runtime.Invocation;
using System;
using System.Reflection;

namespace Super.Runtime.Activation
{
	sealed class Activator : Decorated<Type, object>, IActivator
	{
		public static Activator Default { get; } = new Activator();

		Activator() : base(TypeMetadataSelector.Default
		                                       .Out(YieldSelector<TypeInfo>.Default)
		                                       .Out(ImmutableArraySelector<Type>.Default)
		                                       .Out(new Generic<ISource<object>>(typeof(ActivatorSource<>)))
		                                       .Out(Invoke<ISource<object>>.Default)
		                                       .Out(ValueSelector<object>.Default)) {}
	}

	public sealed class Activator<T> : Decorated<T>, IActivator<T>
	{
		public static Activator<T> Default { get; } = new Activator<T>();

		Activator() : base(Singleton<T>.Default.Or(Activation<T>.Default)) {}
	}
}