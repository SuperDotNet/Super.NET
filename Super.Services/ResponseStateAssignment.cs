using Microsoft.AspNetCore.Http;
using Super.Model.Sources;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;

namespace Super.Services
{
	class ResponseStateAssignment : IAssignable<HttpRequest, HttpClient>
	{
		readonly static Func<HttpClient, System.Net.Http.HttpClientHandler> Handler = AssociatedHandlers.Default.Get;

		readonly Func<HttpClient, System.Net.Http.HttpClientHandler> _handler;
		readonly Func<HttpRequest, Cookie>                           _state;

		public ResponseStateAssignment(Func<HttpRequest, Cookie> state) : this(Handler, state) {}

		public ResponseStateAssignment(Func<HttpClient, System.Net.Http.HttpClientHandler> handler,
		                               Func<HttpRequest, Cookie> state)
		{
			_handler = handler;
			_state   = state;
		}

		public void Execute(KeyValuePair<HttpRequest, HttpClient> parameter)
		{
			_handler(parameter.Value)
				.CookieContainer
				.Add(parameter.Value.BaseAddress, _state(parameter.Key));
		}
	}
}