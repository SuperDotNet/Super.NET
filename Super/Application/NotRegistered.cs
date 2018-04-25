using LightInject;
using Super.Model.Collections;
using Super.Model.Selection;
using Super.Model.Specifications;
using Super.Reflection;
using System;

namespace Super.Application
{
	sealed class NotRegistered : DecoratedSelect<IServiceRegistry, Func<Type, bool>>
	{
		public static NotRegistered Default { get; } = new NotRegistered();

		NotRegistered() : base(In<IServiceRegistry>.Select(x => x.AvailableServices)
		                                           .Out(ServiceTypeSelector.Default)
		                                           .Activate(I<NotHave<Type>>.Default)
		                                           .Out(DelegateSelector<Type>.Default)) {}
	}
}