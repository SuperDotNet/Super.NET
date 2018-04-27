using Super.Model.Selection;
using Super.Reflection;
using Super.Runtime.Activation;
using System;

namespace Super
{
	public static partial class ExtensionMethods
	{
		public static TTo To<TFrom, TTo>(this TFrom @this, I<TTo> select) where TTo : IActivateMarker<TFrom>
			=> select.From(@this);

		public static TTo New<TFrom, TTo>(this I<TTo> _, TFrom parameter)
			=> Runtime.Activation.New<TFrom, TTo>.Default.Get(parameter);

		public static TTo Activate<TFrom, TTo>(this I<TTo> _, TFrom parameter)
			=> Activations<TFrom, TTo>.Default.Get(parameter);

		public static T From<TParameter, T>(this I<T> _, TParameter parameter) where T : IActivateMarker<TParameter>
			=> MarkedActivations<TParameter, T>.Default.Get(parameter);

		public static I<T, TParameter, TResult> Source<T, TParameter, TResult>(this I<T> _,
		                                                                       Func<T, ISelect<TParameter, TResult>> __)
			where T : ISelect<TParameter, TResult> => I<T, TParameter, TResult>.Default;

		/*public static ISelect<TParameter, TResult> Try<TException, TParameter, TResult>(
			this I<TException> _, Func<TParameter, TResult> source, Func<TParameter, TResult> fallback)
			where TException : Exception
			=> ;*/
	}
}