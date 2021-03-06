﻿using System;
using System.Reflection;
using Super.Compose;
using Super.Model.Selection;
using Super.Reflection.Members;

namespace Super.Runtime.Activation
{
	sealed class SingletonPropertyDelegates : Select<PropertyInfo, Func<object>>
	{
		public static SingletonPropertyDelegates Default { get; } = new SingletonPropertyDelegates();

		SingletonPropertyDelegates() : base(Start.A.Selection<PropertyInfo>()
		                                         .By.Calling(x => x.GetMethod)
		                                         .Select(MethodDelegates<Func<object>>.Default)
		                                         .Then()
		                                         .Singleton()
		                                         .Get()) {}
	}
}