using LightInject;
using Super.Model.Collections;
using Super.Model.Selection;
using Super.Model.Specifications;
using Super.Reflection;
using System;

namespace Super.Application
{
	sealed class CanRegister : DecoratedSelect<IServiceRegistry, Func<Type, bool>>
	{
		public static CanRegister Default { get; } = new CanRegister();

		CanRegister() : base(Start.From<IServiceRegistry>()
		                         .Select(x => x.AvailableServices)
		                         .Select(ServiceTypeSelector.Default)
		                         .Select(I<NotHave<Type>>.Default)
		                         .Select(DelegateSelector<Type>.Default)) {}
	}
}