using Super.Model.Sequences;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Super.Reflection.Types
{
	sealed class GenericActivation : ArrayInstance<ParameterExpression>, IGenericActivation
	{
		readonly Array<ParameterExpression> _expressions;
		readonly Array<Type>                _types;

		public GenericActivation(Array<Type> types)
			: this(types, types.Reference().Select(Defaults.Parameter).ToArray()) {}

		public GenericActivation(Array<Type> types, params ParameterExpression[] expressions) : base(expressions)
		{
			_types       = types;
			_expressions = expressions;
		}

		public Expression Get(Type parameter)
		{
			var constructor = parameter.GetTypeInfo().DeclaredConstructors.Only() ??
			                  parameter.GetConstructors().Only() ??
			                  parameter.GetConstructor(_types);
			var types      = constructor.GetParameters().Select(x => x.ParameterType);
			var parameters = _expressions.Reference().Zip(types, Defaults.ExpressionZip);
			var result     = Expression.New(constructor, parameters);
			return result;
		}
	}
}