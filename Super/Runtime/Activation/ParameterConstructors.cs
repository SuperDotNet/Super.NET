using System;
using System.Linq.Expressions;
using System.Reflection;
using Super.Model.Selection;
using Super.Runtime.Invocation.Expressions;

namespace Super.Runtime.Activation
{
	sealed class ParameterConstructors<TIn, TOut> : Invocation.Expressions.Delegates<ConstructorInfo, Func<TIn, TOut>>
	{
		public static ParameterConstructors<TIn, TOut> Default { get; }
			= new ParameterConstructors<TIn, TOut>();

		ParameterConstructors() : this(ConstructorExpressions<TIn>.Default) {}

		public ParameterConstructors(ISelect<ConstructorInfo, Expression> select)
			: base(select, Parameter<TIn>.Default.Get()) {}
	}
}