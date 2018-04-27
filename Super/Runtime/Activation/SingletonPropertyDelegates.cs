using Super.Model.Selection;
using Super.Model.Selection.Alterations;
using Super.Reflection.Members;
using System;
using System.Reflection;

namespace Super.Runtime.Activation
{
	sealed class SingletonPropertyDelegates : DecoratedSelect<PropertyInfo, Func<object>>
	{
		public static SingletonPropertyDelegates Default { get; } = new SingletonPropertyDelegates();

		SingletonPropertyDelegates() : base(PropertyAccessMethodSelector.Default
		                                                                .Select(MethodDelegates<Func<object>>.Default)
		                                                                .Select(SingletonDelegateSelector<object>.Default)) {}
	}
}