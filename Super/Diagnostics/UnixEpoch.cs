using System;
using Super.Model.Sources;

namespace Super.Diagnostics
{
	public sealed class UnixEpoch : Source<DateTime>
	{
		public static UnixEpoch Default { get; } = new UnixEpoch();

		UnixEpoch() : base(new DateTime(1970, 1, 1, 0, 0, 0, 0)) {}
	}
}