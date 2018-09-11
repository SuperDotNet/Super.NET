using Super.Model.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Super.Model.Collections
{
	sealed class InlineSelections<TFrom, TTo>
		: ISelect<Expression<Func<TFrom, TTo>>, Expression<Action<TFrom[], TTo[], int, int>>>
	{
		public static InlineSelections<TFrom, TTo> Default { get; } = new InlineSelections<TFrom, TTo>();

		InlineSelections() : this(Expression.Variable(typeof(int), "index"),
		                          Expression.Parameter(typeof(TFrom[]), "source"),
		                          Expression.Parameter(typeof(TTo[]), "destination"),
		                          Expression.Parameter(typeof(int), "start"),
		                          Expression.Parameter(typeof(int), "finish")) {}

		readonly ParameterExpression                  _index;
		readonly IEnumerable<ParameterExpression>     _input;
		readonly ISelect<string, ParameterExpression> _parameters;

		public InlineSelections(ParameterExpression index, params ParameterExpression[] parameters)
			: this(index, parameters.Hide(), parameters.ToDictionary(x => x.Name).ToTable()) {}

		public InlineSelections(ParameterExpression index, IEnumerable<ParameterExpression> input,
		                        ISelect<string, ParameterExpression> parameters)
		{
			_index      = index;
			_input      = input;
			_parameters = parameters;
		}

		public Expression<Action<TFrom[], TTo[], int, int>> Get(Expression<Func<TFrom, TTo>> parameter)
		{
			var label = Expression.Label();

			var inline = new InlineVisitor(parameter.Parameters[0],
			                               Expression.ArrayAccess(_parameters.Get("source"), _index))
				             .Visit(parameter.Body) ??
			             throw new InvalidOperationException("Inline expression was not found");

			var body = Expression.Block(_index.Yield(),
			                            Expression.Assign(_index,
			                                              Expression.Subtract(_parameters.Get("start"),
			                                                                  Expression.Constant(1))),
			                            Expression.Loop(Expression
				                                            .IfThenElse(Expression.LessThan(Expression.PreIncrementAssign(_index), _parameters.Get("finish")),
				                                                        Expression
					                                                        .Assign(Expression.ArrayAccess(_parameters.Get("destination"), _index),
					                                                                inline),
				                                                        Expression.Break(label)),
			                                            label));

			var result = Expression.Lambda<Action<TFrom[], TTo[], int, int>>(body, _input);
			return result;
		}
	}
}