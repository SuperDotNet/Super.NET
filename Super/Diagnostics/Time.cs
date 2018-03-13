using System;

namespace Super.Diagnostics
{
	public sealed class Time : ITime
	{
		public static Time Default { get; } = new Time();

		Time() {}

		public DateTimeOffset Get() => DateTimeOffset.Now;
	}
}