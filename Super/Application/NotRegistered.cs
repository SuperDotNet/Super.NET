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
		                                           .Select(ServiceTypeSelector.Default)
		                                           .Activate(I<NotHave<Type>>.Default)
		                                           .Select(DelegateSelector<Type>.Default)) {}
	}
}