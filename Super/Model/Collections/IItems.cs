using System.Collections.Generic;
using Super.Model.Selection;
using Super.Model.Sources;

namespace Super.Model.Collections
{
	public interface IArray<TFrom, TTo> : ISelect<Array<TFrom>, Array<TTo>> {}

	/*class ArrayConvert<TFrom, TTo> : IArray<TFrom, TTo>
	{
		readonly Converter<TFrom, TTo> _select;

		/*public ArrayConvert(ISelect<TFrom, TTo> select) : this(select.Get) {}#1#

		public ArrayConvert(Converter<TFrom, TTo> select) => _select = select;

		public Array<TTo> Get(Array<TFrom> parameter) => parameter.Select(_select);
	}*/

	public interface IItems<out T> : ISource<IEnumerable<T>> {}

	/*sealed class Sequence<T> : IReadOnlyCollection<T>
	{
		readonly T[] _array;
		readonly int _length;

		public Sequence(T[] array, int length)
		{
			_array  = array;
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
	}*/
}