using System;
using Polly;
using Super.Diagnostics;
using Super.Model.Commands;

namespace Super.ExtensionMethods
{
	public static class Diagnostics
	{
		public static void Execute<T>(this ICommand<ExceptionParameter<TimeSpan>> @this, DelegateResult<T> result,
		                              TimeSpan span, Context context)
			=> @this.Execute(new ExceptionParameter<TimeSpan>(result.Exception, span));
	}
}