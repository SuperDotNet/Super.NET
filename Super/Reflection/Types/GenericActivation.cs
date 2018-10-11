using System;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Super.Reflection.Types
{
	sealed class GenericActivation : IGenericActivation
	{
		readonly ReadOnlyMemory<ParameterExpression> _expressions;
		readonly ImmutableArray<Type>                _types;

		public GenericActivation(ImmutableArray<Type> types, params ParameterExpression[] expressions)
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
			var memory     = _expressions;
			var parameters = memory.ToArray().Zip(types, Defaults.ExpressionZip);
			var result     = Expression.New(constructor, parameters);
			return result;
		}
	}
}