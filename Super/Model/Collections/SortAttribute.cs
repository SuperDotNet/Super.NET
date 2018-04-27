using Super.Model.Sources;
using System;

namespace Super.Model.Collections
{
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class SortAttribute : Attribute, ISource<int>
	{
		readonly int _sort;

		public SortAttribute(int sort) => _sort = sort;

		public int Get() => _sort;
	}
}