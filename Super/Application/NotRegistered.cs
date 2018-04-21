using LightInject;
using Super.Model.Collections;
using Super.Model.Selection;
using System;

namespace Super.Application
{
	sealed class NotRegistered : DecoratedSelect<IServiceRegistry, Func<Type, bool>>
	{
		public static NotRegistered Default { get; } = new NotRegistered();

		NotRegistered() : base(In<IServiceRegistry>.Select(x => x.AvailableServices)
		                                           .Out(ServiceTypeSelector.Default)
		                                           .Out(New<NotHave<Type>>.Default)
		                                           .Out(x => x.ToDelegate())) {}
	}
}