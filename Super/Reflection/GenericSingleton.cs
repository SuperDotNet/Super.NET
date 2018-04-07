using Super.ExtensionMethods;
using Super.Runtime.Activation;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Super.Reflection
{
	sealed class GenericSingleton : IGenericActivation
	{
		public static GenericSingleton Default { get; } = new GenericSingleton();

		GenericSingleton() : this(Singletons.Default,
		                          typeof(Singletons).GetRuntimeMethod(nameof(Singletons.Get),
		                                                              typeof(Type).Yield().ToArray())) {}

		readonly MethodInfo _method;

		readonly ISingletons _singletons;

		public GenericSingleton(ISingletons singletons, MethodInfo method)
		{
			_singletons = singletons;
			_method     = method;
		}

		public Expression Get(Type parameter)
		{
			var instance = Expression.Constant(_singletons);
			var call     = Expression.Call(instance, _method, Expression.Constant(parameter));
			var result   = Expression.Convert(call, parameter);
			return result;
		}
	}
}