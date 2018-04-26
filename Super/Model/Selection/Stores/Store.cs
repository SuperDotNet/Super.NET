using Super.Runtime.Activation;
using System;

namespace Super.Model.Selection.Stores
{
	class Store<TParameter, TResult> : DecoratedSelect<TParameter, TResult>
	{
		readonly static Func<TParameter, TResult> Source = New<TParameter, TResult>.Default.ToDelegateReference();

		public Store() : this(Source) {}

		public Store(Func<TParameter, TResult> source) : base(Stores<TParameter, TResult>.Default.Get(source)) {}
	}
}