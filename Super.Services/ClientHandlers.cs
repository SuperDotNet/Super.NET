using System;
using Super.ExtensionMethods;
using Super.Model.Sources;
using Super.Model.Sources.Tables;

namespace Super.Services
{
	sealed class ClientHandlers : EqualityStore<Uri, System.Net.Http.HttpClientHandler>
	{
		public static ClientHandlers Default { get; } = new ClientHandlers();

		ClientHandlers() : base(new StandardTables<Uri, System.Net.Http.HttpClientHandler>(_ => new HttpClientHandler())
		                        .Get(RegisteredClientHandlers.Default)
		                        .ToDelegate()) {}
	}
}