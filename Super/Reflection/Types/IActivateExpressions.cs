using System;
using System.Linq.Expressions;
using Super.Model.Selection;
using Super.Model.Sequences;

namespace Super.Reflection.Types
{
	public interface IActivateExpressions : ISelect<Type, Expression>
	{
		IArray<ParameterExpression> Parameters { get; }
	}
}