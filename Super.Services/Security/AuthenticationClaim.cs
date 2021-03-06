﻿using Newtonsoft.Json;

namespace Super.Application.Services.Security
{
	public sealed class AuthenticationClaim
	{
		[JsonProperty("typ")]
		public string Type { get; set; }

		[JsonProperty("val")]
		public string Value { get; set; }
	}
}