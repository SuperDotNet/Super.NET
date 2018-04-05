﻿using Polly;
using Super.Model.Commands;
using System;
using System.Collections.Immutable;
// ReSharper disable TooManyArguments

namespace Super.Diagnostics
{
	public static class Objects
	{
		public static void Execute<T>(this ICommand<ExceptionParameter<TimeSpan>> @this, DelegateResult<T> result,
		                              TimeSpan span)
			=> @this.Execute(new ExceptionParameter<TimeSpan>(result.Exception, span));

		public static void Execute(this ICommand<ExceptionParameter<ImmutableArray<object>>> @this,
		                           System.Exception exception,
		                           params object[] arguments)
			=> @this.Execute(new ExceptionParameter<ImmutableArray<object>>(exception, arguments.ToImmutableArray()));

		public static void Execute<T>(this ICommand<ExceptionParameter<T>> @this, System.Exception exception, T argument)
			=> @this.Execute(new ExceptionParameter<T>(exception, argument));

		public static void Execute<T1, T2>(this ICommand<ExceptionParameter<ValueTuple<T1, T2>>> @this,
		                                   System.Exception exception,
		                                   T1 first, T2 second)
			=> @this.Execute(new ExceptionParameter<ValueTuple<T1, T2>>(exception, ValueTuple.Create(first, second)));

		public static void Execute<T1, T2, T3>(this ICommand<ExceptionParameter<ValueTuple<T1, T2, T3>>> @this,
		                                       System.Exception exception, T1 first, T2 second, T3 third)
			=> @this.Execute(new ExceptionParameter<ValueTuple<T1, T2, T3>>(exception,
			                                                                ValueTuple.Create(first, second, third)));
	}
}