using Microsoft.AspNetCore.Http;
using Super.Model.Sources;

namespace Super.Services
{
	sealed class RequestStateValue : ISource<IRequestCookieCollection, string>
	{
		readonly string _name;

		public RequestStateValue(string name) => _name = name;

		public string Get(IRequestCookieCollection parameter)
			=> parameter.TryGetValue(_name, out var result) ? result : null;
	}
}