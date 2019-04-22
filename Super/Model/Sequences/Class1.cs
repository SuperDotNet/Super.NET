using Super.Model.Results;
using Super.Model.Selection;
using Super.Model.Selection.Stores;
using Super.Runtime;
using Super.Runtime.Activation;
using System;
using System.Collections.Generic;

namespace Super.Model.Sequences
{
	public interface ISequence<T> : IResult<Store<T>> {}

	public static class Sequence
	{
		public static ISequence<T> From<T>(params T[] items) => new Sequence<T>(items);

		public static ISequence<T> From<T>(params IResult<T>[] items) => new DeferredSequence<T>(items);
	}

	class Sequence<T> : ISequence<T>
	{
		readonly Array<T> _items;

		public Sequence(Array<T> items) => _items = items;

		public Store<T> Get() => new Store<T>(_items);
	}

	public class DeferredSequence<T> : ISequence<T>
	{
		readonly Array<IResult<T>> _results;
		readonly IStores<T>        _stores;

		public DeferredSequence(Array<IResult<T>> results) : this(results, Leases<T>.Default) {}

		public DeferredSequence(Array<IResult<T>> results, IStores<T> stores)
		{
			_results = results;
			_stores  = stores;
		}

		public Store<T> Get()
		{
			var length = _results.Length;
			var result = _stores.Get(length);
			for (var i = 0u; i < length; i++)
			{
				result.Instance[i] = _results[i].Get();
			}

			return result;
		}
	}

	public interface IArray<T> : IResult<Array<T>> {}

	public class ArrayInstance<T> : Instance<Array<T>>, IArray<T>
	{
		public ArrayInstance(IEnumerable<T> enumerable) : this(enumerable.Open()) {}

		public ArrayInstance(params T[] instance) : base(instance) {}
	}

	public class ArrayStore<T> : DeferredSingleton<Array<T>>,
	                             IArray<T>,
	                             IActivateUsing<IResult<Array<Type>>>,
	                             IActivateUsing<Func<Array<T>>>
	{
		public ArrayStore(ISelect<None, Array<T>> source) : this(source.Get) {}

		public ArrayStore(Func<Array<T>> source) : base(source) {}
	}

	public class ArrayResult<T> : Results.Result<Array<T>>, IArray<T>
	{
		public ArrayResult(IResult<Array<T>> source) : this(source.Get) {}

		public ArrayResult(Func<Array<T>> source) : base(source) {}
	}

	public interface IArray<in _, T> : ISelect<_, Array<T>> {}

	public class ArrayStore<_, T> : Store<_, Array<T>>, IArray<_, T>
	{
		public ArrayStore(ISelect<_, IEnumerable<T>> source) : this(source.Result()) {}

		public ArrayStore(ISelect<_, Array<T>> source) : this(source.Get) {}

		public ArrayStore(Func<_, Array<T>> source) : base(source) {}
	}

	sealed class Result<T> : Select<IEnumerable<T>, Array<T>>
	{
		public static Result<T> Default { get; } = new Result<T>();

		Result() : base(x => x.Open()) {}
	}
}