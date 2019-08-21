using Microsoft.AspNetCore.Http;
using Super.Model.Selection;

namespace Super.Application.Services.Security
{
	public sealed class Authentication : Select<HttpRequest, AuthenticationInformation>
	{
		public static Authentication Default { get; } = new Authentication();

		Authentication() : base(AuthenticationAddress.Default.Then()
		                                             .Select(ClientStore.Default)
		                                             .Configure(AuthenticationStateAssignment.Default)
		                                             .Select(Api<IAuthentication>.Default)
		                                             .Request(x => x.Current())
		                                             .Query()
		                                             .Only()) {}
	}
}