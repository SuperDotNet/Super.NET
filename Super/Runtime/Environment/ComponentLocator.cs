using Super.Model.Selection;
using Super.Runtime.Activation;
using System;
using Activator = Super.Runtime.Activation.Activator;

namespace Super.Runtime.Environment
{
	sealed class ComponentLocator<T> : FixedActivator<T>, IActivateUsing<ISelect<Type, Type>>
	{
		public static ComponentLocator<T> Default { get; } = new ComponentLocator<T>();

		ComponentLocator() : this(ComponentType.Default) {}

		public ComponentLocator(ISelect<Type, Type> type)
			: base(type.Select(Activator.Default.Assigned()).Then().CastForResult<T>().Get()) {}
	}
}