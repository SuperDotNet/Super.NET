using Super.Model.Collections;
using Super.Model.Selection.Alterations;
using System;
using System.Collections.Generic;

namespace Super.Model.Sequences.Query
{
	class SortAlteration<T> : IAlteration<T[]>
	{
		public static SortAlteration<T> Default { get; } = new SortAlteration<T>();

		SortAlteration() : this(SortComparer<T>.Default) {}

		readonly IComparer<T> _comparer;

		public SortAlteration(IComparer<T> comparer) => _comparer = comparer;

		public T[] Get(T[] parameter)
		{
			Array.Sort(parameter, _comparer);
			return parameter;
		}
	}
}