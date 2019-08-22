using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Serilog;
using Super.Diagnostics.Logging;

namespace Super.Application.Services.Communication
{
	class HttpClientHandler : System.Net.Http.HttpClientHandler
	{
		readonly ILogger _logger;

		public HttpClientHandler() : this(Log<HttpClientHandler>.Default) {}

		public HttpClientHandler(ILogger logger) => _logger = logger;

		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
		                                                       CancellationToken cancellationToken)
		{
			_logger.Debug("Resource request made for {Uri}.", request.RequestUri);
			return base.SendAsync(request, cancellationToken);
		}
	}
}