using Super.ExtensionMethods;
using Super.Model.Sources;
using Super.Runtime;
using System;
using System.Linq.Expressions;

namespace Super.Expressions
{
	class Delegates<TParameter, TResult> : DecoratedSource<TParameter, TResult>
	{
		public Delegates(ISource<TParameter, Expression> source) : this(source, Empty<ParameterExpression>.Array) {}

		public Delegates(ISource<TParameter, Expression> source, params ParameterExpression[] expressions)
			: this(source, ReturnType<TResult>.Default.Get(), expressions) {}

		public Delegates(ISource<TParameter, Expression> source, Type resultType, params ParameterExpression[] parameters)
			: base(ConvertExpression.Default
			                        .Get(resultType)
			                        .Out(new Lambda<TResult>(parameters))
			                        .Out(Compiler<TResult>.Default)
			                        .In(source)) {}
	}
}