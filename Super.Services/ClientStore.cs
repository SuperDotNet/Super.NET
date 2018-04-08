using Super.Model.Selection.Stores;
using System;
using System.Net.Http;

namespace Super.Services
{
	sealed class ClientStore : EqualityStore<Uri, HttpClient>
	{
		public static ClientStore Default { get; } = new ClientStore();

		ClientStore() : base(Clients.Default.Get) {}
	}
}