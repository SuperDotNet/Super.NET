using LightInject;
using Super.ExtensionMethods;
using Super.Model.Collections;
using Super.Model.Containers;
using Super.Model.Sources;
using System;

namespace Super.Application
{
	sealed class NotRegistered : DecoratedSource<IServiceRegistry, Func<Type, bool>>
	{
		public static NotRegistered Default { get; } = new NotRegistered();

		NotRegistered() : base(In<IServiceRegistry>.Select(x => x.AvailableServices)
		                                           .Out(ServiceTypeSelector.Default)
		                                           .Out(New<NotHave<Type>>.Default)
		                                           .Out(x => x.ToDelegate())) {}
	}
}