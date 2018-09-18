using Super.Model.Selection;
using Super.Runtime;
using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Super.Model.Collections
{
	public interface IEnumerate<T> : ISelect<IEnumerator<T>, View<T>> {}

	sealed class Enumerate<T> : IEnumerate<T>
	{
		public static Enumerate<T> Default { get; } = new Enumerate<T>();

		Enumerate() : this(ArrayPool<T>.Shared, ArrayPool<View<T>>.Shared) {}

		readonly ArrayPool<T>       _pool;
		readonly ArrayPool<View<T>> _views;
		readonly uint               _size;

		public Enumerate(ArrayPool<T> pool, ArrayPool<View<T>> views, uint size = 1024)
		{
			_pool  = pool;
			_views = views;
			_size  = size;
		}

		public View<T> Get(IEnumerator<T> parameter)
		{
			if (!parameter.MoveNext())
			{
				return new View<T>(Empty<T>.Array);
			}

			var one = parameter.Current;

			if (!parameter.MoveNext())
			{
				return new View<T>(one);
			}

			var two = parameter.Current;
			if (!parameter.MoveNext())
			{
				return new View<T>(one, two);
			}

			var three = parameter.Current;
			if (!parameter.MoveNext())
			{
				return new View<T>(one, two, three);
			}

			var four = parameter.Current;
			if (!parameter.MoveNext())
			{
				return new View<T>(one, two, three, four);
			}

			var five = parameter.Current;

			var items = _pool.Rent((int)_size);
			items[0] = one;
			items[1] = two;
			items[2] = three;
			items[3] = four;
			items[4] = five;
			var view  = new View<T>(_pool, items);
			var size  = items.Length;
			var count = 5u;
			while (count < size && parameter.MoveNext())
			{
				items[count++] = parameter.Current;
			}

			var seed = view.Resize(count);
			return count < size ? seed : Views(seed).Compile(parameter);
		}

		Views<T> Views(in View<T> first)
		{
			var store = _views.Rent(32);
			store[0] = first;
			return new Views<T>(new View<View<T>>(_views, store));
		}
	}

	sealed class Load<T> : ISelect<IEnumerable, ArraySegment<T>>
	{
		public static Load<T> Default { get; } = new Load<T>();

		Load() : this(Enumerate<T>.Default, ArrayPool<T>.Shared) {}

		readonly IEnumerate<T> _enumerate;
		readonly ArrayPool<T>  _pool;

		public Load(IEnumerate<T> enumerate, ArrayPool<T> pool)
		{
			_enumerate = enumerate;
			_pool      = pool;
		}

		public ArraySegment<T> Get(IEnumerable parameter)
		{
			switch (parameter)
			{
				case T[] array:
					return new ArraySegment<T>(array);
				case ICollection<T> collection:
					var rental = _pool.Rent(collection.Count);
					collection.CopyTo(rental, 0);
					return new ArraySegment<T>(rental, 0, collection.Count);
				default:
					//var enumerable = parameter is IEnumerable<T> e ? e : parameter.OfType<T>();
					return /*_enumerate.Get(enumerable.GetEnumerator())*/new ArraySegment<T>();
			}

		}
	}

	public readonly struct View<T>
	{
		readonly static ArrayPool<T> ArrayPool = ArrayPool<T>.Shared;

		readonly ArraySegment<T> _view;
		readonly ArrayPool<T>    _pool;

		public View(params T[] store) : this(null, store) {}

		public View(ArrayPool<T> pool, params T[] store) : this(pool, new ArraySegment<T>(store)) {}

		public View(ArrayPool<T> pool, ArraySegment<T> view)
		{
			_pool = pool;
			_view = view;

			Used      = (uint)_view.Count;
			Available = (uint)_view.Array.Length;
		}

		public uint Used { get; }

		public uint Available { get; }

		public T[] Source => _view.Array;

		/*public ArraySegment<T> Segment => _view;*/

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Copy(in View<T> source, uint offset)
		{
			Array.ConstrainedCopy(source.Source, 0, Source, (int)offset, (int)source.Used);
			//source._view.CopyTo(_view.Array, offset);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public View<T> New(uint? size = null)
		{
			var used  = (int)(size ?? Used);
			var pool  = _pool ?? ArrayPool;
			return new View<T>(pool, new ArraySegment<T>(pool.Rent(used), _view.Offset, used));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Release()
		{
			_pool?.Return(Source);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public View<T> Resize(uint length) => new View<T>(_pool, new ArraySegment<T>(Source, _view.Offset, (int)length));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public T[] Allocate()
		{
			var result = new T[Used];
			Array.Copy(Source, result, Used);
			Release();
			return result;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public T[] ToArray(Func<ArraySegment<T>, T[]> wat)
		{
			var result = wat(_view);
			Release();
			return result;
		}
	}
}