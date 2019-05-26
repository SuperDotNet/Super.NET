using System;
using System.Collections.Generic;
using Super.Model.Results;
using Super.Model.Selection;
using Super.Model.Selection.Stores;
using Super.Runtime.Activation;

namespace Super.Model.Sequences
{
	public class ArrayStore<T> : DeferredSingleton<Array<T>>,
	                             IArray<T>,
	                             IActivateUsing<IResult<Array<Type>>>,
	                             IActivateUsing<Func<Array<T>>>
	{
		public ArrayStore(IResult<Array<T>> source) : this(source.Get) {}

		public ArrayStore(Func<Array<T>> source) : base(source) {}
	}

	public class ArrayStore<_, T> : Store<_, Array<T>>, IArray<_, T>
	{
		public ArrayStore(ISelect<_, IEnumerable<T>> source) : this(source.Result()) {}

		public ArrayStore(ISelect<_, Array<T>> source) : this(source.Get) {}

		public ArrayStore(Func<_, Array<T>> source) : base(source) {}
	}
}