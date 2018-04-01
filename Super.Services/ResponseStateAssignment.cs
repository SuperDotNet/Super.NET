using System;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Super.Model.Commands;

namespace Super.Services
{
	class ResponseStateAssignment : ICommand<(HttpRequest Parameter, HttpClient Result)>
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

		public void Execute((HttpRequest Parameter, HttpClient Result) parameter)
		{
			_handler(parameter.Result)
				.CookieContainer
				.Add(parameter.Result.BaseAddress, _state(parameter.Parameter));
		}
	}
}