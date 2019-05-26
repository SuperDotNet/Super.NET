using System;
using Super.Model.Results;

namespace Super.Runtime
{
	public sealed class UnixEpoch : Instance<DateTime>
	{
		public static UnixEpoch Default { get; } = new UnixEpoch();

		UnixEpoch() : base(new DateTime(1970, 1, 1, 0, 0, 0, 0)) {}
	}
}