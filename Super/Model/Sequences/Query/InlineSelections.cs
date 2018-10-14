using Super.Model.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Super.Model.Sequences.Query
{
	sealed class InlineSelections<TFrom, TTo>
		: ISelect<Expression<Func<TFrom, TTo>>, Expression<Action<TFrom[], TTo[], uint, uint>>>
	{
		public static InlineSelections<TFrom, TTo> Default { get; } = new InlineSelections<TFrom, TTo>();

		InlineSelections() : this(Expression.Variable(typeof(uint), "index"),
		                          Expression.Parameter(typeof(TFrom[]), "source"),
		                          Expression.Parameter(typeof(TTo[]), "destination"),
		                          Expression.Parameter(typeof(uint), "start"),
		                          Expression.Parameter(typeof(uint), "finish")) {}

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

		public Expression<Action<TFrom[], TTo[], uint, uint>> Get(Expression<Func<TFrom, TTo>> parameter)
		{
			var label = Expression.Label();
			var cast = Expression.Convert(_index, typeof(int));
			var from = Expression.ArrayAccess(_parameters.Get("source"), cast);
			var to   = Expression.ArrayAccess(_parameters.Get("destination"), cast);

			var inline = new InlineVisitor(parameter.Parameters[0], from).Visit(parameter.Body)
			             ??
			             throw new InvalidOperationException("Inline expression was not found");

			var body = Expression.Block(_index.Yield(),
			                            Expression.Assign(_index, Expression.Subtract(_parameters.Get("start"),
			                                                                          Expression.Constant(1u))),
			                            Expression.Loop(Expression
				                                            .IfThenElse(Expression
					                                                        .LessThan(Expression.PreIncrementAssign(_index),
					                                                                  _parameters.Get("finish")),
				                                                        Expression.Assign(to, inline),
				                                                        Expression.Break(label)),
			                                            label));

			var result = Expression.Lambda<Action<TFrom[], TTo[], uint, uint>>(body, _input);
			return result;
		}
	}
}