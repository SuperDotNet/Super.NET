using System;
using Super.Model.Selection;

namespace Super.Diagnostics
{
	public sealed class UnixTime : ISelect<double, DateTime>
	{
		public static UnixTime Default { get; } = new UnixTime();

		UnixTime() : this(UnixEpoch.Default.Get()) {}

		readonly DateTime _epoch;

		public UnixTime(DateTime epoch) => _epoch = epoch;

		public DateTime Get(double parameter)
		{
			var time   = _epoch;
			var result = time.AddSeconds(parameter);
			return result;
		}
	}
}