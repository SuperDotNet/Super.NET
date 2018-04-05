using Super.Model.Instances;
using System;

namespace Super.Runtime
{
	public sealed class Time : DelegatedInstance<DateTimeOffset>, ITime
	{
		public static Time Default { get; } = new Time();

		Time() : base(() => DateTimeOffset.Now) {}
	}
}