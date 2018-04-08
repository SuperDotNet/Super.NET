using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Super.Model.Selection;

namespace Super.Runtime.Invocation.Expressions
{
	sealed class Parameters<T> : Parameters
	{
		public static Parameters<T> Default { get; } = new Parameters<T>();

		Parameters() : base(Parameter<T>.Default.Get()) {}
	}

	class Parameters : ISelect<ConstructorInfo, IEnumerable<Expression>>
	{
		readonly ISelect<Type, Expression> _parameter;

		public Parameters(ParameterExpression expression) : this(new ConvertParameter(expression)) {}

		public Parameters(ISelect<Type, Expression> parameter) => _parameter = parameter;

		public IEnumerable<Expression> Get(ConstructorInfo parameter)
		{
			var parameters = parameter.GetParameters();
			var result = parameters.Skip(1)
			                       .Select(x => Expression.Constant(x.DefaultValue, x.ParameterType))
			                       .Prepend(_parameter.Get(parameters.First().ParameterType));
			return result;
		}

		sealed class ConvertParameter : ISelect<Type, Expression>
		{
			readonly ParameterExpression _parameter;

			public ConvertParameter(ParameterExpression parameter) => _parameter = parameter;

			public Expression Get(Type parameter) => ConvertExpressions.Default.Get(parameter).Get(_parameter);
		}
	}
}