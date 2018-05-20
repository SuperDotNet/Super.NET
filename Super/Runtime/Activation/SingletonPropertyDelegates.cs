using System;
using System.Reflection;
using Super.Model.Selection;
using Super.Reflection.Members;

namespace Super.Runtime.Activation
{
	sealed class SingletonPropertyDelegates : DecoratedSelect<PropertyInfo, Func<object>>
	{
		public static SingletonPropertyDelegates Default { get; } = new SingletonPropertyDelegates();

		SingletonPropertyDelegates() : base(PropertyAccessMethodSelector.Default
		                                                                .Select(MethodDelegates<Func<object>>.Default)
		                                                                .Singleton()) {}
	}
}