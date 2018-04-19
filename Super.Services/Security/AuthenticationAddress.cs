using Microsoft.AspNetCore.Http;
using Super.Model.Selection.Stores;
using Super.Runtime;
using System;

namespace Super.Services.Security
{
	sealed class AuthenticationAddress : ReferenceStore<HttpRequest, Uri>
	{
		public static AuthenticationAddress Default { get; } = new AuthenticationAddress();

		AuthenticationAddress() : base(AuthenticationBaseAddress.Default
		                                                        .Select(Uris.Default.Assigned())
		                                                        .Or(CurrentRequestUri.Default
		                                                                             .Out(Authority.Default)
		                                                                             .Out(Uris.Default))
		                                                        .ToDelegate()) {}
	}
}