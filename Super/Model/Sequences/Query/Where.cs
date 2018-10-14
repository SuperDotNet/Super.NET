using Super.Model.Specifications;
using System;

namespace Super.Model.Sequences.Query
{
	// TODO: Reconcile.

	public class Where<T> : ISelector<T, T>
	{
		readonly Func<T, bool> _where;

		public Where(ISpecification<T> where) : this(where.IsSatisfiedBy) {}

		public Where(Func<T, bool> where) => _where = where;

		public Array<T> Get(Array<T> parameter)
		{
			var length = parameter.Length;

			Span<int> indexes = stackalloc int[(int)length];
			var       count   = 0;
			for (var i = 0; i < length; i++)
			{
				var element = parameter[i];
				if (_where(element))
				{
					indexes[count++] = i;
				}
			}

			var result = new T[count];
			for (var i = 0; i < count; i++)
			{
				result[i] = parameter[indexes[i]];
			}

			return result;
		}
	}
}