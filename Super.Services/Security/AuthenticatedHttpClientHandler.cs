﻿using Flurl;
using Super.Model.Selection.Alterations;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using HttpClientHandler = Super.Application.Services.Communication.HttpClientHandler;

namespace Super.Application.Services.Security
{
	sealed class AuthenticatedHttpClientHandler : HttpClientHandler
	{
		readonly string        _apiKey;
		readonly Alter<string> _secret;

		public AuthenticatedHttpClientHandler(string apiKey, string secret) : this(apiKey, new Encryptor(secret).Get) {}

		public AuthenticatedHttpClientHandler(string apiKey, Alter<string> secret)
		{
			_apiKey = apiKey;
			_secret = secret;
		}

		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
		                                                       CancellationToken cancellationToken)
		{
			var uri = request.RequestUri
			                 .ToString()
			                 .SetQueryParam("apikey", _apiKey)
			                 .SetQueryParam("nonce", (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds)
			                 .ToString();
			request.RequestUri = new UriBuilder(uri).Uri;
			var parameter = request.RequestUri.ToString();
			var secret    = _secret(parameter);
			request.Headers.Add("apisign", secret);

			var result = base.SendAsync(request, cancellationToken);
			return result;
		}
	}
}