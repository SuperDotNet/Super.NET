using Super.ExtensionMethods;
using Super.Reflection;
using Super.Runtime.Activation;
using System;

namespace Super.Model.Sources
{
	public static class From<T>
	{
		public static ISource<T, TResult> To<TResult>(Func<T, TResult> @this)
			=> @this.To(I<DelegatedSource<T, TResult>>.Default);

		public static ISource<T, TResult> To<TResult>(I<TResult> _ = null)
			where TResult : IActivateMarker<T> => Activations<T, TResult>.Default;
	}

	public static class From
	{
		public static ISource<TParameter, TResult> New<TParameter, TResult>(I<TResult> infer = null)
			where TResult : IActivateMarker<TParameter>
			=> From<TParameter>.To(infer);
	}
}
