using System;

namespace Super.Runtime.Execution
{
	public struct Details
	{
		public Details(string name) : this(name, Time.Default) {}

		public Details(string name, DateTimeOffset observed)
		{
			Name     = name;
			Observed = observed;
		}

		public string Name { get; }

		public DateTimeOffset Observed { get; }
	}
}