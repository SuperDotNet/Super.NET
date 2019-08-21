using Super.Compose;
using Super.Model.Selection.Stores;
using System;

namespace Super.Application.Services
{
	sealed class ClientHandlers : EqualityStore<Uri, System.Net.Http.HttpClientHandler>
	{
		public static ClientHandlers Default { get; } = new ClientHandlers();

		ClientHandlers() : base(Start.A.Selection<Uri>().AndOf<HttpClientHandler>().By.Instantiation,
		                        Start.An.Instance<RegisteredClientHandlers>()) {}
	}
}