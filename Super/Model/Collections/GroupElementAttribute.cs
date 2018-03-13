using System;
using Super.Model.Instances;

namespace Super.Model.Collections
{
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class GroupElementAttribute : Attribute, IInstance<string>
	{
		readonly string _name;

		public GroupElementAttribute(string name) => _name = name;

		public string Get() => _name;
	}
}