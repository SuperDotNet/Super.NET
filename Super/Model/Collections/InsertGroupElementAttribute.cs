using System;
using Super.Model.Instances;

namespace Super.Model.Collections
{
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class InsertGroupElementAttribute : Attribute, IInstance<int>
	{
		readonly int _index;

		public InsertGroupElementAttribute() : this(0) {}

		public InsertGroupElementAttribute(int index) => _index = index;

		public int Get() => _index;
	}
}