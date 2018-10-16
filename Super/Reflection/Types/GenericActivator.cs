using Super.Model.Selection.Stores;
using Super.Runtime.Invocation;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Super.Reflection.Types
{
	class GenericActivator<T> : Store<Type, T> where T : Delegate
	{
		public GenericActivator(params Type[] types) : this(new GenericActivation(types)) {}

		public GenericActivator(IGenericActivation activation)
			: base(new Invocation(activation.Get().Reference()).Select(x => x.Compile()).To(activation.Select).Get) {}

		sealed class Invocation : Invocation0<Expression, IEnumerable<ParameterExpression>, Expression<T>>
		{
			public Invocation(IEnumerable<ParameterExpression> parameter) : base(Expression.Lambda<T>, parameter) {}
		}
	}
}