using System;
using Microsoft.AspNetCore.Http;
using Super.ExtensionMethods;
using Super.Model.Sources;
using Super.Runtime;

namespace Super.Services.Security
{
	sealed class AuthenticationAddress : ReferenceStore<HttpRequest, Uri>
	{
		public static AuthenticationAddress Default { get; } = new AuthenticationAddress();

		AuthenticationAddress() : base(AuthenticationBaseAddress.Default
		                                                        .Allow()
		                                                        .Assigned(Uris.Default)
		                                                        .Or(CurrentRequestUri.Default
		                                                                             .Out(Authority.Default)
		                                                                             .Out(Uris.Default))
		                                                        .ToDelegate()) {}
	}
}