using Super.Model.Selection;
using Super.Model.Sources;
using Super.Reflection.Types;
using Super.Runtime.Environment;
using System;
using System.Diagnostics.Contracts;

namespace Super.Model.Collections
{
	public readonly struct Array<T> : ISource<T[]>
	{
		internal readonly T[] _source;

		public Array(T[] source) : this(source, source.Length) {}

		public Array(T[] source, int length)
		{
			_source = source;
			Length  = length;
		}

		public T this[int index] => _source[index];

		public int Length { get; }

		[Pure]
		public T[] Get() => _source;
	}

	static class Implementations
	{
		public static ISelect<Type, uint> Size { get; } = DefaultComponent<ISize>.Default.Get().ToStore();
	}

	sealed class Size<T> : FixedDeferredSingleton<Type, uint>
	{
		public static Size<T> Default { get; } = new Size<T>();

		Size() : base(Implementations.Size, Type<T>.Instance) {}
	}

	public interface ISize : ISelect<Type, uint> {}



	sealed class EnhancedArraySelect<TFrom, TTo> : ISelect<EnhancedArray<TFrom>, Array<TTo>>
	{
		readonly Func<TFrom, TTo> _select;

		public EnhancedArraySelect(ISelect<TFrom, TTo> select) : this(select.Get) {}

		public EnhancedArraySelect(Func<TFrom, TTo> select) => _select = select;

		public Array<TTo> Get(EnhancedArray<TFrom> parameter)
		{
			var source = parameter.Get();
			var length = source.Length;
			var store  = new TTo[length];
			for (var i = 0; i < length; ++i)
			{
				store[i] = _select(source[i]);
			}

			var result = new Array<TTo>(store);
			return result;
		}
	}

	public sealed class EnhancedArray<T>
	{
		const int I = sizeof(int);

		readonly static int Size = checked((int)Size<T>.Default.Get()) * I + 12;

		readonly T[] _source;

		T     _target;
		int[] _storage;
		int   _currentIndex = -1;

		public EnhancedArray(T[] source) : this(source, source.Length) {}

		public EnhancedArray(T[] source, int length)
		{
			_source  = source;
			Length   = length;
			_storage = new int[length * Size];
			for (var i = 0; i < Length; i++)
			{
				Add(source[i]);
			}
		}

		public void Add(T item)
		{
			_currentIndex++;
			unsafe
			{
				var reference   = __makeref(item);
				var itemAddress = (int*)(*(int*)*(int*)&reference - I);
				for (var i = 0; i < Size; ++i)
				{
					var value = *itemAddress;
					_storage[_currentIndex * Size + i] = value;
					itemAddress                        = itemAddress + 1;
				}
			}
		}

		public T this[int index]
		{
			get
			{
				unsafe
				{
					var arrayReference  = __makeref(_storage);
					var targetReference = __makeref(_target);
					*(int*)*(int*)&targetReference = *(int*)*(int*)&arrayReference + index * Size;
					return _target;
				}
			}
		}

		public int Length { get; }

		[Pure]
		public T[] Get() => _source;
	}
}