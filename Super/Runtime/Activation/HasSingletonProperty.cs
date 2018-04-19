using System;
using System.Reflection;
using Super.Model.Specifications;

namespace Super.Runtime.Activation
{
	public sealed class HasSingletonProperty : HasResult<Type, PropertyInfo>
	{
		public static HasSingletonProperty Default { get; } = new HasSingletonProperty();

		HasSingletonProperty() : base(SingletonProperty.Default.ToDelegate()) {}
	}
}