using Microsoft.AspNetCore.Http;
using Super.Compose;
using Super.Model.Selection;

namespace Super.Services.Security
{
	public sealed class Authentication : DecoratedSelect<HttpRequest, AuthenticationInformation>
	{
		public static Authentication Default { get; } = new Authentication();

		Authentication() : base(A.Of<AuthenticationAddress>()
		                         .Select(ClientStore.Default)
		                         .Select(A.Of<AuthenticationStateAssignment>())
		                         .Select(A.Of<Api<IAuthentication>>())
		                         .Request(x => x.Current())
		                         .Query()
		                         .Only()) {}
	}
}