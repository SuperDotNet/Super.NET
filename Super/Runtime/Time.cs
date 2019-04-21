using Super.Model.Results;
using System;

namespace Super.Runtime
{
	public sealed class Time : DelegatedResult<DateTimeOffset>, ITime
	{
		public static Time Default { get; } = new Time();

		Time() : base(() => DateTimeOffset.Now) {}
	}
}