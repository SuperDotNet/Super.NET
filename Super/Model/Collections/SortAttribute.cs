using System;
using Super.Model.Instances;

namespace Super.Model.Collections
{
	[AttributeUsage(AttributeTargets.Class)]
	sealed class SortAttribute : Attribute, IInstance<int>
	{
		readonly int _sort;

		public SortAttribute(int sort) => _sort = sort;

		public int Get() => _sort;
	}
}