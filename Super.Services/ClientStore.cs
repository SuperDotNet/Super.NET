using System;
using System.Net.Http;
using Super.Model.Sources;

namespace Super.Services
{
	sealed class ClientStore : EqualityStore<Uri, HttpClient>
	{
		public static ClientStore Default { get; } = new ClientStore();

		ClientStore() : base(Clients.Default.Get) {}
	}
}