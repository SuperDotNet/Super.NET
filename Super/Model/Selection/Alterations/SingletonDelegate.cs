using Super.Model.Results;
using Super.Model.Sequences;
using System;

namespace Super.Model.Selection.Alterations
{
	public sealed class SingletonDelegate<T> : Alteration<Func<T>>
	{
		public static SingletonDelegate<T> Default { get; } = new SingletonDelegate<T>();

		SingletonDelegate() : base(x => new DeferredSingleton<T>(x).Get) {}
	}

	public class Aggregate<TElement, T> : IAlteration<T> where TElement : ISelect<T, T>
	{
		readonly Func<Array<TElement>> _items;

		public Aggregate(IResult<Array<TElement>> items) : this(items.Get) {}

		public Aggregate(Func<Array<TElement>> items) => _items = items;

		public T Get(T parameter)
		{
			var items  = _items();
			var count  = items.Length;
			var result = parameter;
			for (var i = 0u; i < count; i++)
			{
				result = items[i].Get(result);
			}

			return result;
		}
	}

	public class Aggregate<T> : Aggregate<ISelect<ISelect<T, T>, ISelect<T, T>>, ISelect<T, T>>
	{
		public Aggregate(IResult<Array<ISelect<ISelect<T, T>, ISelect<T, T>>>> items) : this(items.Get) {}

		public Aggregate(Func<Array<ISelect<ISelect<T, T>, ISelect<T, T>>>> items) : base(items) {}
	}
}