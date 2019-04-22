using Super.Runtime.Activation;
using System;

namespace Super.Model.Selection.Stores
{
	public class ActivatedStore<TIn, TOut> : Store<TIn, TOut>
	{
		protected ActivatedStore() : base(New<TIn, TOut>.Default.ToDelegateReference()) {}
	}

	public class Store<TIn, TOut> : Select<TIn, TOut>
	{
		protected Store(Func<TIn, TOut> source) : base(Stores<TIn, TOut>.Default.Get(source)) {}
	}
}