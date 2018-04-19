using Super.Model.Collections;
using Super.Model.Selection;
using Super.Model.Sources;
using Super.Reflection.Types;
using Super.Runtime.Invocation;
using System;
using System.Reflection;

namespace Super.Runtime.Activation
{
	sealed class Activator : DecoratedSelect<Type, object>, IActivator
	{
		public static Activator Default { get; } = new Activator();

		Activator() : base(TypeMetadataSelector.Default
		                                       .Out(YieldSelector<TypeInfo>.Default)
		                                       .Out(ImmutableArraySelector<Type>.Default)
		                                       .Out(new Generic<ISource<object>>(typeof(ActivatorSource<>)))
		                                       .Out(Call<ISource<object>>.Default)
		                                       .Out(ValueSelector<object>.Default)) {}
	}

	public sealed class Activator<T> : DecoratedSource<T>, IActivator<T>
	{
		public static Activator<T> Default { get; } = new Activator<T>();

		Activator() : base(Singleton<T>.Default.Or(Activation<T>.Default)) {}
	}
}