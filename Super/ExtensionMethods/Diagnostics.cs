using Polly;
using Serilog;
using Super.Diagnostics;
using Super.Model.Commands;
using Super.Reflection;
using Super.Runtime.Activation;
using System;

// ReSharper disable TooManyArguments

namespace Super.ExtensionMethods
{
	public static class Diagnostics
	{
		public static T Load<T>(this ILogger @this) where T : IActivateMarker<ILogger>
			=> @this.To(I<T>.Default);


		public static void Execute<T>(this ICommand<ExceptionParameter<TimeSpan>> @this, DelegateResult<T> result,
		                              TimeSpan span, Context context)
			=> @this.Execute(new ExceptionParameter<TimeSpan>(result.Exception, span));
	}
}