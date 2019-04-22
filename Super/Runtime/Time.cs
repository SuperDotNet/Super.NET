using Super.Model.Results;
using System;

namespace Super.Runtime
{
	public sealed class Time : Result<DateTimeOffset>, ITime
	{
		public static Time Default { get; } = new Time();

		Time() : base(() => DateTimeOffset.Now) {}
	}
}