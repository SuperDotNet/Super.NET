using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Super.Application.Services
{
	class HttpClientHandler : System.Net.Http.HttpClientHandler
	{
		readonly ILogger _logger;

		public HttpClientHandler() : this(new LoggerFactory().CreateLogger<HttpClientHandler>()) {}

		public HttpClientHandler(ILogger logger) => _logger = logger;

		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
		                                                       CancellationToken cancellationToken)
		{
			_logger.LogDebug("Resource request made for {Uri}.", request.RequestUri);
			return base.SendAsync(request, cancellationToken);
		}
	}
}