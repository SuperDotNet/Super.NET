using System.Diagnostics.Contracts;
using Super.Model.Sources;

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
}