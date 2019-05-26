using System;
using Microsoft.AspNetCore.Http;
using Super.Compose;
using Super.Model.Selection.Stores;
using Super.Runtime;

namespace Super.Services.Security
{
	sealed class AuthenticationAddress : ReferenceValueStore<HttpRequest, Uri>
	{
		public static AuthenticationAddress Default { get; } = new AuthenticationAddress();

		AuthenticationAddress() : base(Start.An.Instance<CurrentRequestUri>()
		                                    .Select(Authority.Default)
		                                    .Select(Uris.Default)
		                                    .Unless(A.Of<Uris>()
		                                             .Assigned()
		                                             .In(A.Of<AuthenticationBaseAddress>().Get)
		                                             .ToDelegate())
		                                    .Get) {}
	}
}