using Super.Model.Sources;
using System;
using System.Linq.Expressions;

namespace Super.Reflection
{
	public interface IGenericActivation : ISource<Type, Expression> {}
}