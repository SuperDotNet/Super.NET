using System;
using Super.Model.Results;

namespace Super.Runtime
{
	public sealed class Time : Result<DateTimeOffset>, ITime
	{
		public static Time Default { get; } = new Time();

		Time() : base(() => DateTimeOffset.Now) {}
	}
}