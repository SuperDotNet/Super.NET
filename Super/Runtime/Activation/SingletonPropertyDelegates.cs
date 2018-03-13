using System;
using System.Reflection;
using Super.ExtensionMethods;
using Super.Model.Sources;
using Super.Model.Sources.Alterations;
using Super.Reflection;

namespace Super.Runtime.Activation
{
	sealed class SingletonPropertyDelegates : DecoratedSource<PropertyInfo, Func<object>>
	{
		public static SingletonPropertyDelegates Default { get; } = new SingletonPropertyDelegates();

		SingletonPropertyDelegates() : base(MethodDelegates<Func<object>>.Default
		                                                                 .In(PropertyMethodCoercer.Default)
		                                                                 .Out(SingletonDelegateCoercer<object>.Default)) {}
	}
}