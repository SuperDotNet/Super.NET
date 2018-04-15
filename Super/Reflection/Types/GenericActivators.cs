using Super.Model.Selection;
using System;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;

namespace Super.Reflection.Types
{
	sealed class GenericActivators<T> : ISelect<Type, T>
	{
		readonly IGenericActivation                  _activation;
		readonly ImmutableArray<ParameterExpression> _expressions;

		public GenericActivators(params Type[] types) : this(types.ToImmutableArray()) {}

		public GenericActivators(ImmutableArray<Type> types)
			: this(types, types.Select(Defaults.Parameter).ToImmutableArray()) {}

		public GenericActivators(ImmutableArray<Type> types, ImmutableArray<ParameterExpression> expressions)
			: this(new GenericActivation(types, expressions), expressions) {}

		public GenericActivators(IGenericActivation activation) : this(activation,
		                                                               ImmutableArray<ParameterExpression>.Empty) {}

		public GenericActivators(IGenericActivation activation, ImmutableArray<ParameterExpression> expressions)
		{
			_activation  = activation;
			_expressions = expressions;
		}

		public T Get(Type parameter) => Expression.Lambda<T>(_activation.Get(parameter), _expressions.ToArray())
		                                          .Compile();
	}
}