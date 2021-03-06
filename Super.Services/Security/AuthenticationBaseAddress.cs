﻿using Super.Runtime;

namespace Super.Application.Services.Security
{
	sealed class AuthenticationBaseAddress : EnvironmentVariable
	{
		public static AuthenticationBaseAddress Default { get; } = new AuthenticationBaseAddress();

		AuthenticationBaseAddress() : base(nameof(AuthenticationBaseAddress)) {}
	}
}