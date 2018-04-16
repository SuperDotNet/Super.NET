using Super.Model.Commands;
using System;
using System.Collections.Generic;

namespace Super.Model.Collections
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

	public class InsertIntoList<T> : ICommand<IList<T>>
	{
		readonly T _item;
		readonly Func<IList<T>, int> _index;

		public InsertIntoList(T item) : this(item, x => 0) {}

		public InsertIntoList(T item, Func<IList<T>, int> index)
		{
			_item = item;
			_index = index;
		}

		public void Execute(IList<T> parameter)
		{
			parameter.Insert(_index(parameter), _item);
		}
	}
}