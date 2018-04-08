﻿using Microsoft.AspNetCore.Http;
using Super.ExtensionMethods;
using Super.Model.Sources;

namespace Super.Services.Security
{
	public sealed class Authentication : DecoratedSource<HttpRequest, AuthenticationInformation>
	{
		public static Authentication Default { get; } = new Authentication();

		Authentication() : base(AuthenticationAddress.Default
		                                             .Out(ClientStore.Default)
		                                             .Configure(AuthenticationStateAssignment.Default)
		                                             .Out(Api<IAuthentication>.Default)
		                                             .Request(x => x.Current())
		                                             .Out(x => x.Only())) {}
	}
}