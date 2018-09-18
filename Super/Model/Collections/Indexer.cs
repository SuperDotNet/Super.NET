using Super.Model.Commands;
using Super.Model.Selection;
using Super.Runtime;
using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Super.Model.Collections
{
	public interface IEnumerate<T> : ISelect<IEnumerator<T>, ArrayView<T>> {}

	sealed class Enumerate<T> : IEnumerate<T>
	{
		readonly static ArrayView<T> Empty = new ArrayView<T>(Empty<T>.Array);

		public static Enumerate<T> Default { get; } = new Enumerate<T>();

		Enumerate() : this(Lease<ArrayView<T>>.Default, Lease<T>.Default) {}

		readonly ILease<ArrayView<T>> _items;
		readonly ILease<T>            _item;
		readonly uint                 _size;

		public Enumerate(ILease<ArrayView<T>> items, ILease<T> item, uint size = 1024)
		{
			_items = items;
			_item  = item;
			_size  = size;
		}

		static ArrayView<T> Get(params T[] items) => new ArrayView<T>(items);

		public ArrayView<T> Get(IEnumerator<T> parameter)
		{
			if (!parameter.MoveNext())
			{
				return Empty;
			}

			var one = parameter.Current;

			if (!parameter.MoveNext())
			{
				return Get(one);
			}

			var two = parameter.Current;
			if (!parameter.MoveNext())
			{
				return Get(one, two);
			}

			var three = parameter.Current;
			if (!parameter.MoveNext())
			{
				return Get(one, two, three);
			}

			var four = parameter.Current;
			if (!parameter.MoveNext())
			{
				return Get(one, two, three, four);
			}

			var five = parameter.Current;

			var view  = _item.Get(_size);
			var items = view.Array;
			items[0] = one;
			items[1] = two;
			items[2] = three;
			items[3] = four;
			items[4] = five;
			var size  = items.Length;
			var count = 5u;
			while (count < size && parameter.MoveNext())
			{
				items[count++] = parameter.Current;
			}

			return count < size ? view.Resize(count) : Compile(view, parameter);
		}

		ArrayView<T> Compile(in ArrayView<T> first, IEnumerator<T> parameter)
		{
			using (var store = _items.Get(32))
			{
				store.Array[0] = first;
				var result = new DynamicArray<T>(_item, store.Array).Get(parameter);
				return result;
			}
		}
	}

	[StructLayout(LayoutKind.Sequential, Size = 18, Pack = 2)]
	public readonly struct ArrayView<T> : IDisposable
	{
		public ArrayView(T[] array) : this(array, 0, (uint)array.Length) {}

		// ReSharper disable once TooManyDependencies
		public ArrayView(T[] array, uint start, uint length, bool clear = false)
		{
			Array   = array;
			Start   = start;
			Length = length;

			_clear = clear;

		}


		public T[] Array { get; }

		public uint Start { get; }

		public uint Length { get; }
		readonly bool _clear;


		public ArrayView<T> Resize(uint size) => Resize(Start, size);

		public ArrayView<T> Resize(uint start, uint size)
			// => start != Start || size != Length ? new ArrayView<T>(Array, start, size, _return) : this;
		=> new ArrayView<T>(Array, start, size);

		public Array<T> Get() => new Array<T>(Start == 0 && Length == Array.Length ? Array : Release());

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public T[] Release()
		{
			if (Start > 0 || Length != Array.Length)
			{
				var result = new T[Length];
				System.Array.ConstrainedCopy(Array, (int)Start, result, 0, (int)Length);
				Dispose();
				return result;
			}

			return Array;
		}


		public void Dispose()
		{
			//_return?.Invoke(Array);
		}
	}

	public interface ILease<T> : ISelect<uint, ArrayView<T>> {}

	public interface IRelease<in T> : ICommand<T[]> {}

	sealed class Release<T> : IRelease<T>
	{
		public static Release<T> Default { get; } = new Release<T>();

		Release() : this(ArrayPool<T>.Shared) {}

		readonly ArrayPool<T> _pool;

		public Release(ArrayPool<T> pool) => _pool = pool;

		public void Execute(T[] parameter)
		{
			_pool.Return(parameter);
		}
	}

	sealed class Lease<T> : ILease<T>
	{
		public static Lease<T> Default { get; } = new Lease<T>();

		Lease() : this(ArrayPool<T>.Shared, Release<T>.Default.Execute) {}

		readonly ArrayPool<T> _pool;
		readonly Action<T[]>  _release;

		public Lease(ArrayPool<T> pool, Action<T[]> release)
		{
			_pool    = pool;
			_release = release;
		}

		public ArrayView<T> Get(uint parameter) => new ArrayView<T>(_pool.Rent((int)parameter), 0, parameter);
	}

	sealed class Load<T> : ISelect<IEnumerable, ArrayView<T>>
	{
		public static Load<T> Default { get; } = new Load<T>();

		Load() : this(Enumerate<T>.Default, Lease<T>.Default) {}

		readonly IEnumerate<T> _enumerate;
		readonly ILease<T>     _lease;

		public Load(IEnumerate<T> enumerate, ILease<T> lease)
		{
			_enumerate = enumerate;
			_lease     = lease;
		}

		public ArrayView<T> Get(IEnumerable parameter)
		{
			switch (parameter)
			{
				case T[] array:
					return new ArrayView<T>(array);
				/*case ICollection<T> collection:
					var lease = _lease.Get(collection.Count);
					collection.CopyTo(lease.Array, 0);
					return lease;
				default:
					var enumerable = parameter is IEnumerable<T> e ? e : parameter.OfType<T>();
					return _enumerate.Get(enumerable.GetEnumerator());*/
			}
			throw new IOException();
		}
	}

	/*public readonly struct View<T>
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

		/*public ArraySegment<T> Segment => _view;#1#

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Copy(in View<T> source, uint offset)
		{
			Array.ConstrainedCopy(source.Source, 0, Source, (int)offset, (int)source.Used);
			//source._view.CopyTo(_view.Array, offset);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public View<T> New(uint? size = null)
		{
			var used = (int)(size ?? Used);
			var pool = _pool ?? ArrayPool;
			return new View<T>(pool, new ArraySegment<T>(pool.Rent(used), _view.Offset, used));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Release()
		{
			_pool?.Return(Source);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public View<T> Resize(uint length)
			=> new View<T>(_pool, new ArraySegment<T>(Source, _view.Offset, (int)length));

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
	}*/
}