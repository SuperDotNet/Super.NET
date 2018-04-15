using System;
using System.Linq.Expressions;
using Super.Model.Selection;

namespace Super.Reflection.Types
{
	public interface IGenericActivation : ISelect<Type, Expression> {}
}