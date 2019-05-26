using System;
using Super.Model.Results;

namespace Super.Model.Sequences.Collections
{
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class SortAttribute : Attribute, IResult<int>
	{
		readonly int _sort;

		public SortAttribute(int sort) => _sort = sort;

		public int Get() => _sort;
	}
}