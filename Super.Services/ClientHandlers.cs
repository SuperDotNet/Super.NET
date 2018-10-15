using Super.Model.Selection.Stores;
using System;

namespace Super.Services
{
	sealed class ClientHandlers : EqualityStore<Uri, System.Net.Http.HttpClientHandler>
	{
		public static ClientHandlers Default { get; } = new ClientHandlers();

		ClientHandlers() : base(Start.From<Uri>().New<HttpClientHandler>(), RegisteredClientHandlers.Default) {}
	}
}