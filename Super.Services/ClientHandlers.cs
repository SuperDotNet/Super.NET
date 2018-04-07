using Super.ExtensionMethods;
using Super.Model.Sources;
using Super.Model.Sources.Tables;
using Super.Runtime.Activation;
using System;

namespace Super.Services
{
	sealed class ClientHandlers : EqualityStore<Uri, System.Net.Http.HttpClientHandler>
	{
		public static ClientHandlers Default { get; } = new ClientHandlers();

		ClientHandlers() : base(new StandardTables<Uri, System.Net.Http.HttpClientHandler>(Activator<HttpClientHandler>.Default.Any)
		                        .Get(RegisteredClientHandlers.Default)) {}
	}
}