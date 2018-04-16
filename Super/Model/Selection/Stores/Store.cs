using System;
using Super.ExtensionMethods;
using Super.Runtime.Activation;

namespace Super.Model.Selection.Stores
{
	class Store<TParameter, TResult> : Select<TParameter, TResult>
	{
		public Store() : this(Activation<TParameter, TResult>.Default.ToDelegate()) {}

		public Store(Func<TParameter, TResult> source) : base(Stores<TParameter, TResult>.Default.Get(source).Get) {}
	}
}