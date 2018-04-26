using Super.Model.Sources;
using System;

namespace Super.Runtime
{
	public sealed class Time : DelegatedSource<DateTimeOffset>, ITime
	{
		public static Time Default { get; } = new Time();

		Time() : base(() => DateTimeOffset.Now) {}
	}
}