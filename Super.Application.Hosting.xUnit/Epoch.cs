﻿using System;
using Super.Model.Results;

namespace Super.Application.Hosting.xUnit
{
	public sealed class Epoch : Instance<DateTimeOffset>
	{
		public static Epoch Default { get; } = new Epoch();

		Epoch() : base(new DateTimeOffset(1976, 6, 7, 11, 18, 24,
		                                  TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time")
		                                              .BaseUtcOffset)) {}
	}
}