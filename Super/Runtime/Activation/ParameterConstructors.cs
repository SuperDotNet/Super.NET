using System;
using System.Linq.Expressions;
using System.Reflection;
using Super.Expressions;
using Super.Model.Sources;

namespace Super.Runtime.Activation
{
	sealed class ParameterConstructors<TParameter, TResult> : Delegates<ConstructorInfo, Func<TParameter, TResult>>
	{
		public static ParameterConstructors<TParameter, TResult> Default { get; }
			= new ParameterConstructors<TParameter, TResult>();

		ParameterConstructors() : this(Instances<TParameter>.Default) {}

		public ParameterConstructors(ISource<ConstructorInfo, Expression> source)
			: base(source, Parameter<TParameter>.Default.Get()) {}
	}
}