using System;
using Super.Model.Sources;

namespace Super.Runtime
{
	public sealed class Time : DelegatedSource<DateTimeOffset>, ITime
	{
		public static Time Default { get; } = new Time();

		Time() : base(() => DateTimeOffset.Now) {}
	}
}