using Microsoft.AspNetCore.Http;
using Super.Model.Selection;

namespace Super.Application.Services.Communication
{
	sealed class RequestStateValue : ISelect<IRequestCookieCollection, string>
	{
		readonly string _name;

		public RequestStateValue(string name) => _name = name;

		public string Get(IRequestCookieCollection parameter)
			=> parameter.TryGetValue(_name, out var result) ? result : null;
	}
}