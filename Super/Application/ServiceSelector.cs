using System;
using Super.Model.Selection;

namespace Super.Application
{
	sealed class ServiceSelector<T> : Select<IServiceProvider, T>
	{
		public static ServiceSelector<T> Default { get; } = new ServiceSelector<T>();

		ServiceSelector() : base(x => x.Get<T>()) {}
	}
}