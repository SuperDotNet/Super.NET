using Super.Model.Selection;
using Super.Model.Sources;
using Super.Reflection.Types;
using System;

namespace Super.Runtime.Activation
{
	sealed class Activator : DecoratedSelect<Type, object>, IActivator
	{
		public static Activator Default { get; } = new Activator();

		Activator() : base(Self<Type>.Default
		                             .Out(new Generic<ISource<object>>(typeof(ReferenceActivator<>)))
		                             .Value()) {}
	}

	public sealed class Activator<T> : DecoratedSource<T>, IActivator<T>
	{
		public static Activator<T> Default { get; } = new Activator<T>();

		Activator() : base(New<T>.Default.Unless(Singleton<T>.Default)) {}
	}

	public class FixedActivator<T> : FixedSelection<Type, T>, IActivator<T>
	{
		public FixedActivator(ISelect<Type, T> select) : base(select, Type<T>.Instance) {}
	}
}