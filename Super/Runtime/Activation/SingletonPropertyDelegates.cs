using Super.ExtensionMethods;
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

		SingletonPropertyDelegates() : base(MethodDelegates<Func<object>>.Default
		                                                                 .In(PropertyAccessMethodSelector.Default)
		                                                                 .Out(SingletonDelegateSelector<object>.Default)) {}
	}
}