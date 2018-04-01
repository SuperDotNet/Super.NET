using Microsoft.AspNetCore.Http;
using Super.Model.Sources;

namespace Super.Services
{
	sealed class RequestStateCoercer : DelegatedSource<HttpRequest, IRequestCookieCollection>
	{
		public static RequestStateCoercer Default { get; } = new RequestStateCoercer();

		RequestStateCoercer() : base(x => x.Cookies) {}
	}
}