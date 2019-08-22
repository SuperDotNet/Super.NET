using System;
using System.Collections.Concurrent;

namespace Super.Application.Services.Communication
{
	sealed class RegisteredClientHandlers : ConcurrentDictionary<Uri, System.Net.Http.HttpClientHandler>
	{
		public static RegisteredClientHandlers Default { get; } = new RegisteredClientHandlers();

		RegisteredClientHandlers() {}
	}
}