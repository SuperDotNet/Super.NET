using Super.Model.Selection;
using Super.Model.Sources;
using Super.Reflection.Types;
using System;

namespace Super.Runtime.Activation
{
	sealed class Activator : DecoratedSelect<Type, object>, IActivator
	{
		public static Activator Default { get; } = new Activator();

		Activator() : base(TypeMetadataSelector.Default
		                                       .Exit(new Generic<ISource<object>>(typeof(ActivatorSource<>)))
		                                       .Value()) {}
	}

	public sealed class Activator<T> : DecoratedSource<T>, IActivator<T>
	{
		public static Activator<T> Default { get; } = new Activator<T>();

		Activator() : base(Singleton<T>.Default.Or(New<T>.Default)) {}
	}

	public class FixedActivator<T> : FixedSelection<Type, T>, IActivator<T>
	{
		public FixedActivator(ISelect<Type, T> select) : base(select, Type<T>.Instance) {}
	}
}