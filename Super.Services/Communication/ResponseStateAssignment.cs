﻿using System;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Super.Model.Commands;
using Super.Runtime;

namespace Super.Application.Services.Communication
{
	class ResponseStateAssignment : IAssign<HttpRequest, HttpClient>
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

		public void Execute(Pair<HttpRequest, HttpClient> parameter)
		{
			_handler(parameter.Value)
				.CookieContainer
				.Add(parameter.Value.BaseAddress, _state(parameter.Key));
		}
	}
}