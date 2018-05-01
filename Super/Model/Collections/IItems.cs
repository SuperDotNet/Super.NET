using Super.Model.Sources;
using System.Collections;
using System.Collections.Generic;

namespace Super.Model.Collections
{
	/*public interface IArray<T> : ISource<Array<T>> {}

	public struct Array<T>
	{
		readonly T[] _source;

		public Array(T[] source) => _source = source;


	}*/

	public interface IItems<out T> : ISource<IEnumerable<T>> {}

	sealed class Sequence<T> : IReadOnlyCollection<T>
	{
		readonly T[] _array;
		readonly int _length;

		public Sequence(T[] array, int length)
		{
			_array = array;
			_length = length;
		}

		public IEnumerator<T> GetEnumerator()
		{
			for (var i = 0; i < _length; i++)
			{
				yield return _array[i];
			}
		}

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public int Count => _array.Length;
	}
}