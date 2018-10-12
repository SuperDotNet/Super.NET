using Microsoft.AspNetCore.Http;
using Super.Model.Selection;

namespace Super.Services.Security
{
	public sealed class Authentication : DecoratedSelect<HttpRequest, AuthenticationInformation>
	{
		public static Authentication Default { get; } = new Authentication();

		Authentication() : base(AuthenticationAddress.Default
		                                             .Select(ClientStore.Default)
		                                             .Configure(AuthenticationStateAssignment.Default)
		                                             .Select(Api<IAuthentication>.Default)
		                                             .Request(x => x.Current())
		                                             .Result()
		                                             .Only()) {}
	}
}