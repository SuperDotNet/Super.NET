using System;
using System.Linq.Expressions;
using Super.Model.Selection;

namespace Super.Reflection
{
	public interface IGenericActivation : ISelect<Type, Expression> {}
}