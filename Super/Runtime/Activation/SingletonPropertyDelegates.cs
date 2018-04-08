using Super.ExtensionMethods;
using Super.Reflection;
using System;
using System.Reflection;
using Super.Model.Selection;
using Super.Model.Selection.Alterations;

namespace Super.Runtime.Activation
{
	sealed class SingletonPropertyDelegates : Decorated<PropertyInfo, Func<object>>
	{
		public static SingletonPropertyDelegates Default { get; } = new SingletonPropertyDelegates();

		SingletonPropertyDelegates() : base(MethodDelegates<Func<object>>.Default
		                                                                 .In(PropertyAccessMethodSelector.Default)
		                                                                 .Out(SingletonDelegateSelector<object>.Default)) {}
	}
}