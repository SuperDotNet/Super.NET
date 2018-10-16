using Super.Model.Selection;
using Super.Model.Sequences;
using System;
using System.Linq.Expressions;

namespace Super.Reflection.Types
{
	public interface IGenericActivation : ISelect<Type, Expression>, IArray<ParameterExpression> {}
}