using System;
using Super.ExtensionMethods;
using Super.Runtime.Activation;

namespace Super.Model.Sources
{
	class Store<TParameter, TResult> : DelegatedSource<TParameter, TResult>
	{
		public Store() : this(New<TParameter, TResult>.Default.ToDelegate()) {}

		public Store(Func<TParameter, TResult> source) : base(Stores<TParameter, TResult>.Default.Get(source).Get) {}
	}
}