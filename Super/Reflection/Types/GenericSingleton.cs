using Super.Runtime.Activation;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Super.Reflection.Types
{
	sealed class GenericSingleton : IGenericActivation
	{
		public static GenericSingleton Default { get; } = new GenericSingleton();

		GenericSingleton() : this(typeof(Singletons).GetRuntimeMethod(nameof(Singletons.Get),
		                                                              typeof(Type).Yield().ToArray())) {}

		readonly MethodInfo _method;

		public GenericSingleton(MethodInfo method) => _method = method;

		public Expression Get(Type parameter)
		{
			var call = Expression.Call(Expression.Constant(Singletons.Default), _method,
			                           Expression.Constant(parameter));
			var result = Expression.Convert(call, parameter);
			return result;
		}
	}
}