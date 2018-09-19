using Super.Model.Selection;
using Super.Model.Sources;
using Super.Reflection.Types;
using Super.Runtime;
using Super.Runtime.Environment;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Super.Model.Collections
{
	public readonly struct Array<T> : IReadOnlyList<T>
	{
		public static Array<T> Empty { get; } = new Array<T>(Empty<T>.Array);

		public static implicit operator ImmutableArray<T>(Array<T> source) => source.Get();

		public static implicit operator T[](Array<T> source) => source.Copy();

		readonly T[] _source;

		public Array(T[] source) : this(source, (uint)source.Length) {}

		public Array(T[] source, uint length)
		{
			_source = source;
			Length  = length;
		}

		public uint Length { get; }

		public ref readonly T this[uint index] => ref _source[index];

		public T[] Reference() => _source;

		public T[] Copy() => _source.ToArray();

		public ImmutableArray<T> Get() => ImmutableArray.Create(_source);

		public IEnumerator<T> GetEnumerator() => _source.Hide().GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public int Count => _source.Length;

		public T this[int index] => _source[index];
	}

	/*public interface IIterator<T>
	{
		bool Next(ref int index, out T element);

		uint? Length { get; }
	}

	sealed class Iterator<TFrom, TTo> : IIterator<TTo>
	{
		readonly IIterator<TFrom> _iterator;
		readonly Func<TFrom, TTo> _select;

		public Iterator(IIterator<TFrom> iterator, Func<TFrom, TTo> select) : this(iterator, @select, iterator.Length) {}

		public Iterator(IIterator<TFrom> iterator, Func<TFrom, TTo> select, uint? length)
		{
			Length = length;
			_iterator = iterator;
			_select = @select;
		}

		public bool Next(ref int index, out TTo element)
		{
			var result = _iterator.Next(ref index, out var parameter);
			element = result ? _select(parameter) : default;
			return result;
		}

		public uint? Length { get; }
	}

	sealed class ArrayIterator<T> : IIterator<T>
	{
		readonly T[] _instance;
		readonly uint _length;

		public ArrayIterator(T[] instance) : this(instance, (uint)instance.Length) {}

		public ArrayIterator(T[] instance, uint length)
		{
			_instance = instance;
			_length = length;
		}

		public bool Next(ref int index, out T element)
		{
			var result = index < _length;
			element = result ? _instance[index] : default;
			return result;
		}

		public uint? Length => _length;
	}*/

	static class Implementations
	{
		public static ISelect<Type, uint> Size { get; } = DefaultComponent<ISize>.Default.Emit().ToStore();
	}

	sealed class Size<T> : FixedDeferredSingleton<Type, uint>
	{
		public static Size<T> Default { get; } = new Size<T>();

		Size() : base(Implementations.Size, Type<T>.Instance) {}
	}

	public interface ISize : ISelect<Type, uint> {}
}