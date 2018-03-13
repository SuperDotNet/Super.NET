using System;
using Super.Model.Sources;

namespace Super.Platform
{
	public sealed class Activation : ISource<Type, object>
	{
		public static Activation Default { get; } = new Activation();

		Activation() : this(TypeLocator.Default.Get, Activator.CreateInstance) {}

		readonly Func<Type, object> _activate;

		readonly Func<Type, Type> _type;

		public Activation(Func<Type, Type> type, Func<Type, object> activate)
		{
			_type     = type;
			_activate = activate;
		}

		public object Get(Type parameter) => _activate(_type(parameter));
	}
}