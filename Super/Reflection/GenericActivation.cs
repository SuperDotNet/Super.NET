using Super.ExtensionMethods;
using System;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Super.Reflection
{
	sealed class GenericActivation : IGenericActivation
	{
		readonly ImmutableArray<ParameterExpression> _expressions;
		readonly ImmutableArray<Type>                _types;

		public GenericActivation(ImmutableArray<Type> types, ImmutableArray<ParameterExpression> expressions)
		{
			_types       = types;
			_expressions = expressions;
		}

		public Expression Get(Type parameter)
		{
			var constructor = parameter.GetTypeInfo().DeclaredConstructors.Only() ??
			                  parameter.GetConstructors().Only() ??
			                  parameter.GetConstructor(_types.ToArray());
			var types = constructor.GetParameters()
			                       .Select(x => x.ParameterType);
			var parameters = _expressions.AsEnumerable().Zip(types, Defaults.ExpressionZip);
			var result     = Expression.New(constructor, parameters);
			return result;
		}
	}
}