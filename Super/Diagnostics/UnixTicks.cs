using System;
using Super.Model.Sources;

namespace Super.Diagnostics
{
	public sealed class UnixTicks : ISource<DateTime, double>
	{
		public static UnixTicks Default { get; } = new UnixTicks();

		UnixTicks() : this(UnixEpoch.Default.Get()) {}

		readonly DateTime _epoch;

		public UnixTicks(DateTime epoch) => _epoch = epoch;

		public double Get(DateTime parameter)
		{
			var diff   = parameter - _epoch;
			var result = Math.Floor(diff.TotalSeconds);
			return result;
		}
	}
}