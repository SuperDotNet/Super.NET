using Super.Model.Collections;
using Super.Model.Selection;
using Super.Model.Sources;
using System;

namespace Super.Model.Sequences
{
	public interface IQuery<in _, T> : ISource<ISelect<_, T[]>>,
	                                   ISelect<IAlterState<Collections.Selection>, IQuery<_, T>>,
	                                   ISelect<ISegment<T>, IQuery<_, T>> {}

	/*public interface ISelection<T> : IStructure<ArrayView<T>, Session<T>> {}*/

	/*public interface IAlterSelection<T> : IAlteration<ISelection<T>> {}*/

	sealed class Skip : IAlterState<Collections.Selection>
	{
		readonly uint _skip;

		public Skip(uint skip) => _skip = skip;

		public Collections.Selection Get(Collections.Selection parameter)
			=> new Collections.Selection(parameter.Start + _skip, parameter.Length);
	}

	sealed class Take : IAlterState<Collections.Selection>
	{
		readonly uint _take;

		public Take(uint take) => _take = take;

		public Collections.Selection Get(Collections.Selection parameter)
			=> new Collections.Selection(parameter.Start, _take);
	}

	sealed class Where<T> : ISegment<T>
	{
		readonly Func<T, bool> _where;

		public Where(Func<T, bool> where) => _where = @where;

		public ArrayView<T> Get(in ArrayView<T> parameter)
		{
			var used  = parameter.Start + parameter.Length;
			var array = parameter.Array;
			var count = 0u;
			for (var i = parameter.Start; i < used; i++)
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