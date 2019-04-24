using Super.Model.Selection.Conditions;
using Super.Model.Sequences;
using System;

namespace Super.Runtime.Activation
{
	public class ServiceProvider : IServiceProvider, ICondition<Type>
	{
		readonly Array<object> _services;
		readonly uint _length;

		public ServiceProvider(params object[] services) : this(new Array<object>(services)) {}

		public ServiceProvider(Array<object> services) : this(services, services.Length) {}

		ServiceProvider(Array<object> services, uint length)
		{
			_services = services;
			_length = length;
		}

		public object GetService(Type serviceType)
		{
			for (var i = 0u; i < _length; i++)
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
			for (var i = 0u; i < _length; i++)
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