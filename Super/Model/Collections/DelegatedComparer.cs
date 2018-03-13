using System;
using System.Collections.Generic;

namespace Super.Model.Collections
{
	class DelegatedComparer<T> : IComparer<T>
	{
		readonly Func<T, int> _sort;

		public DelegatedComparer(Func<T, int> select) => _sort = select;

		public int Compare(T x, T y) => _sort(x).CompareTo(_sort(y));
	}
}