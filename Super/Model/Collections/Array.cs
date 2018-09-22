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