using System;
using System.Collections.Immutable;
using Super.Model.Selection.Conditions;

namespace Super.Runtime.Activation
{
	public class ServiceProvider : IServiceProvider, ICondition<Type>
	{
		readonly ImmutableArray<object> _services;
		readonly int _length;

		public ServiceProvider(params object[] services) : this(services.ToImmutableArray()) {}

		public ServiceProvider(ImmutableArray<object> services) : this(services, services.Length) {}

		ServiceProvider(ImmutableArray<object> services, int length)
		{
			_services = services;
			_length = length;
		}

		public object GetService(Type serviceType)
		{
			for (var i = 0; i < _length; i++)
			{
				var item = _services[i];
				if (serviceType.IsInstanceOfType(item))
				{
					return item;
				}
			}

			return null;
		}

		public bool Get(Type parameter)
		{
			for (var i = 0; i < _length; i++)
			{
				if (parameter.IsInstanceOfType(_services[i]))
				{
					return true;
				}
			}

			return false;
		}
	}
}