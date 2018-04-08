using System;
using System.Linq.Expressions;
using System.Reflection;
using Super.Model.Selection;
using Super.Runtime.Invocation.Expressions;

namespace Super.Runtime.Activation
{
	sealed class ParameterConstructors<TParameter, TResult> : Invocation.Expressions.Delegates<ConstructorInfo, Func<TParameter, TResult>>
	{
		public static ParameterConstructors<TParameter, TResult> Default { get; }
			= new ParameterConstructors<TParameter, TResult>();

		ParameterConstructors() : this(Instances<TParameter>.Default) {}

		public ParameterConstructors(ISelect<ConstructorInfo, Expression> @select)
			: base(@select, Parameter<TParameter>.Default.Get()) {}
	}
}