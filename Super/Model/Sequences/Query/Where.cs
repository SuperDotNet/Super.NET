using System;

namespace Super.Model.Sequences.Query
{
	public class Where<T> : ISelectView<T>
	{
		readonly Func<T, bool> _where;

		public Where(Func<T, bool> where) => _where = @where;

		public ArrayView<T> Get(ArrayView<T> parameter)
		{
			var to    = parameter.Start + parameter.Length;
			var array = parameter.Array;
			var count = 0u;
			for (var i = parameter.Start; i < to; i++)
			{
				var item = array[i];
				if (_where(item))
				{
					array[count++] = item;
				}
			}

			return parameter.Resize(0, count);
		}
	}
}