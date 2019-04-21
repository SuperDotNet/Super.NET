using Super.Model.Selection;
using Super.Model.Sequences;
using System;
using System.Linq.Expressions;

namespace Super.Reflection.Types
{
	public interface IActivateExpressions : ISelect<Type, Expression>
	{
		IArray<ParameterExpression> Parameters { get; }
	}
}