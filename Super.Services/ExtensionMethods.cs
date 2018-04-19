using System;
using Super.Model.Selection;

namespace Super.Services
{
	public static class ExtensionMethods
	{
		public static T Response<T>(this IObservable<T> @this) => Request<T>.Default.Get(@this);

		public static ISelect<TParameter, TResult> Request<TParameter, T, TResult>(
			this ISelect<TParameter, T> @this, Func<T, IObservable<TResult>> parameter)
			=> @this.Out(new Request<T, TResult>(parameter))
			        .Out(Request<TResult>.Default);
	}
}