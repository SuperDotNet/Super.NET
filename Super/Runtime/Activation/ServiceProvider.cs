using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Super.Model.Specifications;

namespace Super.Runtime.Activation
{
	sealed class ServiceProvider : IServiceProvider, ISpecification<Type>
	{
		readonly IList<object> _services;

		public ServiceProvider(params object[] services) : this(services.ToList()) {}

		public ServiceProvider(IList<object> services) => _services = services;

		public object GetService(Type serviceType)
		{
			var info   = serviceType.GetTypeInfo();
			var length = _services.Count;

			for (var i = 0; i < length; i++)
			{
				var item = _services[i];
				if (info.IsInstanceOfType(item))
				{
					return item;
				}
			}

			return null;
		}

		public bool IsSatisfiedBy(Type parameter)
		{
			var length = _services.Count;
			for (var i = 0; i < length; i++)
			{
				var item = _services[i];
				if (parameter.IsInstanceOfType(item))
				{
					return true;
				}
			}

			return false;
		}
	}
}