using Super.Model.Selection;
using Super.Model.Specifications;
using Super.Runtime.Activation;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;

namespace Super.Model.Collections
{
	public class Selector<TFrom, TTo> : ISelector<TFrom, TTo>
	{
		readonly Func<TFrom, TTo> _select;

		public Selector(ISelect<TFrom, TTo> select) : this(select.Get) {}

		public Selector(Func<TFrom, TTo> select) => _select = select;

		public ReadOnlyMemory<TTo> Get(ReadOnlyMemory<TFrom> parameter)
		{
			var length = parameter.Length;
			var result = new TTo[length];
			var span   = parameter.Span;

			for (var i = 0; i < length; i++)
			{
				result[i] = _select(span[i]);
			}

			return result;
		}
	}

	/*public class Selection<TFrom, TTo> //: ISelection<TFrom, TTo>
	{
		readonly static ArrayPool<TTo> Pool = ArrayPool<TTo>.Shared;

		readonly Func<TFrom, TTo> _select;

		/*public Selection(ISelect<TFrom, TTo> select) : this(select.Get) {}#1#

		public Selection(Func<TFrom, TTo> select) => _select = select;

		public View<TTo> Get(View<TFrom> parameter)
		{
			var length = (int)parameter.Used;
			var store = Pool.Rent(length);

			for (var i = 0u; i < length; i++)
			{
				store[i] = _select(parameter[i]);
			}

			parameter.Release();

			return new View<TTo>(store, store.AsMemory(length), Pool);
		}
	}*/

	/*public class Rent<TFrom, TTo> : ISelector<TFrom, TTo>
	{
		readonly Func<TFrom, TTo> _select;

		public Rent(ISelect<TFrom, TTo> select) : this(select.Get) {}

		public Rent(Func<TFrom, TTo> select) => _select = select;

		public ReadOnlyMemory<TTo> Get(ReadOnlyMemory<TFrom> parameter)
		{
			var length = parameter.Length;
			var result = ArrayPool<TTo>.Shared.Rent(length);
			var span   = parameter.Span;

			for (var i = 0; i < length; i++)
			{
				result[i] = _select(span[i]);
			}

			return result;
		}
	}*/

	public class WhereSelection<T> : ISelection<T, T>
	{
		readonly static ArrayPool<T> Pool = ArrayPool<T>.Shared;

		readonly Func<T, bool> _where;

		public WhereSelection(Func<T, bool> where) => _where = @where;

		public View<T> Get(View<T> parameter)
		{
			var used = (int)parameter.Used;

			//var peek = parameter.Peek();
			var store = Pool.Rent(used);

			var length = store.Length;
			var count  = 0u;
			for (var i = 0u; i < length; i++)
			{
				var item = parameter[i];
				if (_where(item))
				{
					store[count++] = item;
				}
			}

			parameter.Release();

			var result = new View<T>(store, store.AsMemory(0, (int)count), Pool);
			return result;
		}
	}

	public class Where<TIn, TOut> : ISelect<TIn, ReadOnlyMemory<TOut>>
	{
		readonly ISelect<TIn, ReadOnlyMemory<TOut>> _select;
		readonly Func<TOut, bool>                   _where;

		public Where(ISelect<TIn, ReadOnlyMemory<TOut>> select, ISpecification<TOut> where)
			: this(select, where.IsSatisfiedBy) {}

		public Where(ISelect<TIn, ReadOnlyMemory<TOut>> select, Func<TOut, bool> where)
		{
			_select = @select;
			_where  = @where;
		}

		public ReadOnlyMemory<TOut> Get(TIn parameter)
		{
			var items  = _select.Get(parameter);
			var length = items.Length;

			Span<int> indexes = stackalloc int[length];
			var       count   = 0;
			var       source  = items.Span;
			for (var i = 0; i < length; i++)
			{
				var element = source[i];
				if (_where(element))
				{
					indexes[count++] = i;
				}
			}

			var result = new TOut[count];
			for (var i = 0; i < count; i++)
			{
				result[i] = source[indexes[i]];
			}

			//ArrayPool<TOut>.Shared.Return(items.Array());

			return result;
		}
	}

	public class Where<T> : ISelector<T, T>
	{
		readonly Func<T, bool> _where;

		public Where(ISpecification<T> where) : this(where.IsSatisfiedBy) {}

		public Where(Func<T, bool> where) => _where = where;

		public ReadOnlyMemory<T> Get(ReadOnlyMemory<T> parameter)
		{
			var length = parameter.Length;

			Span<int> indexes = stackalloc int[length];
			var       count   = 0;
			var       source  = parameter.Span;
			for (var i = 0; i < length; i++)
			{
				var element = source[i];
				if (_where(element))
				{
					indexes[count++] = i;
				}
			}

			var result = new T[count];
			for (var i = 0; i < count; i++)
			{
				result[i] = source[indexes[i]];
			}

			return result;
		}
	}

	class SelectSelector<TFrom, TTo> : ISelect<IEnumerable<TFrom>, IEnumerable<TTo>>, IActivateMarker<Func<TFrom, TTo>>
	{
		readonly Func<TFrom, TTo> _select;

		public SelectSelector(Func<TFrom, TTo> select) => _select = select;

		public IEnumerable<TTo> Get(IEnumerable<TFrom> parameter) => parameter.Select(_select);
	}

	class SelectManySelector<TFrom, TTo> : ISelect<IEnumerable<TFrom>, IEnumerable<TTo>>,
	                                       IActivateMarker<Func<TFrom, IEnumerable<TTo>>>
	{
		readonly Func<TFrom, IEnumerable<TTo>> _select;

		public SelectManySelector(Func<TFrom, IEnumerable<TTo>> select) => _select = select;

		public IEnumerable<TTo> Get(IEnumerable<TFrom> parameter) => parameter.SelectMany(_select);
	}
}