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

		GenericSingleton() : this(Expression.Constant(Singletons.Default),
		                          typeof(Singletons).GetRuntimeMethod(nameof(Singletons.Get),
		                                                              typeof(Type).Yield().ToArray())) {}

		readonly MethodInfo _method;

		readonly Expression _singletons;

		public GenericSingleton(Expression singletons, MethodInfo method)
		{
			_singletons = singletons;
			_method     = method;
		}

		public Expression Get(Type parameter)
		{
			var call     = Expression.Call(_singletons, _method, Expression.Constant(parameter));
			var result   = Expression.Convert(call, parameter);
			return result;
		}
	}
}