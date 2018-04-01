using Super.ExtensionMethods;
using Super.Model.Collections;
using Super.Model.Instances;
using Super.Model.Sources;
using Super.Reflection;
using System;
using System.Reflection;

namespace Super.Runtime.Activation
{
	sealed class Activator : DecoratedSource<Type, object>, IActivator
	{
		public static Activator Default { get; } = new Activator();

		Activator() : base(TypeMetadataCoercer.Default
		                                      .Out(YieldCoercer<TypeInfo>.Default)
		                                      .Out(ImmutableArrayCoercer<TypeInfo>.Default)
		                                      .Out(new Generic<IInstance<object>>(typeof(ActivatorInstance<>)))
		                                      .Invoke()
		                                      .Out(InstanceCoercer<object>.Default)) {}
	}

	public sealed class Activator<T> : DecoratedInstance<T>, IActivator<T>
	{
		public static Activator<T> Default { get; } = new Activator<T>();

		Activator() : base(Singleton<T>.Default.Adapt().Or(New<T>.Default.Adapt())) {}
	}
}