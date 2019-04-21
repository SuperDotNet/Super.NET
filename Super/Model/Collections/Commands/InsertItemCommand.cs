using System;
using System.Collections.Generic;
using Super.Model.Commands;

namespace Super.Model.Collections.Commands
{
	public class InsertItemCommand<T> : ICommand<T>
	{
		readonly Func<T, int> _index;
		readonly IList<T>     _list;

		public InsertItemCommand(IList<T> list) : this(list, x => 0) {}

		public InsertItemCommand(IList<T> list, Func<T, int> index)
		{
			_list  = list;
			_index = index;
		}

		public void Execute(T parameter)
		{
			_list.Insert(_index(parameter), parameter);
		}
	}
}