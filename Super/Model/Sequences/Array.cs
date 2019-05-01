using JetBrains.Annotations;
using Super.Model.Results;
using Super.Model.Selection;
using Super.Reflection.Types;
using Super.Runtime;
using Super.Runtime.Environment;
using System;
using System.Collections.Immutable;

namespace Super.Model.Sequences
{
	public readonly struct Array<T> : IResult<ImmutableArray<T>>
	{
		public static Array<T> Empty { get; } = new Array<T>(Empty<T>.Array);

		public static implicit operator ImmutableArray<T>(Array<T> source) => source.Get();

		public static implicit operator Array<T>(T[] source) => new Array<T>(source);

		public static implicit operator T[](Array<T> source) => source.Copy();

		readonly T[] _reference;

		public Array(params T[] elements) : this(elements, (uint)elements.Length) {}

		public Array(T[] reference, uint length)
		{
			_reference = reference;
			Length     = length;
		}

		public uint Length { get; }

		public ref readonly T this[uint index] => ref _reference[index];

		[Pure]
		public T[] Copy() => Arrays<T>.Default.Get(_reference);

		[Pure]
		public ImmutableArray<T> Get() => ImmutableArray.Create(_reference);

		[Pure]
		public T[] Open() => _reference;
	}

	sealed class Size<T> : FixedSelectedSingleton<Type, uint>
	{
		public static Size<T> Default { get; } = new Size<T>();

		Size() : base(DefaultComponentLocator<ISize>.Default.Assume(), Type<T>.Instance) {}
	}

	public interface ISize : ISelect<Type, uint> {}
}