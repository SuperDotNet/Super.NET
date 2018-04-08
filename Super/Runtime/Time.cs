using System;
using Super.Model.Sources;

namespace Super.Runtime
{
	public sealed class Time : Delegated<DateTimeOffset>, ITime
	{
		public static Time Default { get; } = new Time();

		Time() : base(() => DateTimeOffset.Now) {}
	}
}