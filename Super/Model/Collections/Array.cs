using Super.Model.Selection;
using Super.Model.Sequences;
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
	public readonly struct Array<T> : ISource<ImmutableArray<T>>, IReadOnlyList<T>
	{
		public static Array<T> Empty { get; } = new Array<T>(Empty<T>.Array);

		public static implicit operator ImmutableArray<T>(Array<T> source) => source.Get();

		public static implicit operator Array<T>(T[] source) => new Array<T>(source);

		public static implicit operator T[](Array<T> source) => source.Copy();

		readonly T[] _reference;
		readonly ImmutableArray<T> _array;

		public Array(T[] reference) : this(reference, ImmutableArray.Create(reference), (uint)reference.Length) {}

		public Array(T[] reference, ImmutableArray<T> array, uint length)
		{
			_reference = reference;
			_array = array;
			Length  = length;
		}

		public uint Length { get; }

		public ref readonly T this[uint index] => ref _reference[index];

		public T[] Copy() => ArraySelector<T>.Default.Get(_reference);

		public ImmutableArray<T> Get() => _array;

		public IEnumerable<T> Reference() => _reference;

		public IEnumerator<T> GetEnumerator() => _reference.Hide().GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public int Count => _reference.Length;

		public T this[int index] => _reference[index];
	}

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