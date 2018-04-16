using Microsoft.AspNetCore.Http;
using Super.Model.Selection;

namespace Super.Services
{
	sealed class RequestStateSelector : Select<HttpRequest, IRequestCookieCollection>
	{
		public static RequestStateSelector Default { get; } = new RequestStateSelector();

		RequestStateSelector() : base(x => x.Cookies) {}
	}
}