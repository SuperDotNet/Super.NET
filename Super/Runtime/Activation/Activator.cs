using Super.ExtensionMethods;
using Super.Model.Collections;
using Super.Model.Instances;
using Super.Model.Sources;
using Super.Reflection;
using Super.Runtime.Invocation;
using System;
using System.Reflection;

namespace Super.Runtime.Activation
{
	sealed class Activator : DecoratedSource<Type, object>, IActivator
	{
		public static Activator Default { get; } = new Activator();

		Activator() : base(TypeMetadataCoercer.Default
		                                      .Out(YieldCoercer<TypeInfo>.Default)
		                                      .Out(ImmutableArraySelector<Type>.Default)
		                                      .Out(new Generic<IInstance<object>>(typeof(ActivatorInstance<>)))
		                                      .Out(Invoke<IInstance<object>>.Default)
		                                      .Out(InstanceValueCoercer<object>.Default)) {}
	}

	public sealed class Activator<T> : DecoratedInstance<T>, IActivator<T>
	{
		public static Activator<T> Default { get; } = new Activator<T>();

		Activator() : base(Singleton<T>.Default.Or(Activation<T>.Default)) {}
	}
}