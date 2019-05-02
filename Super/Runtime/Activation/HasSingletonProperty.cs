using System;
using System.Reflection;

namespace Super.Runtime.Activation
{
	public sealed class HasSingletonProperty : IsAssigned<Type, PropertyInfo>
	{
		public static HasSingletonProperty Default { get; } = new HasSingletonProperty();

		HasSingletonProperty() : base(SingletonProperty.Default) {}
	}
}