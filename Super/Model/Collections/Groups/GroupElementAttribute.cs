using Super.Model.Results;
using System;

namespace Super.Model.Collections.Groups
{
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class GroupElementAttribute : Attribute, IResult<string>
	{
		readonly string _name;

		public GroupElementAttribute(string name) => _name = name;

		public string Get() => _name;
	}
}