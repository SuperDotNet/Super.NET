﻿using System;
using System.Net.Http;
using Super.Model.Selection;
using Super.Model.Selection.Stores;

namespace Super.Application.Services.Communication
{
	sealed class Clients : ISelect<Uri, HttpClient>
	{
		public static Clients Default { get; } = new Clients();

		Clients() : this(AssociatedHandlers.Default, ClientHandlers.Default.Get) {}

		readonly ITable<HttpClient, System.Net.Http.HttpClientHandler> _associated;
		readonly Func<Uri, System.Net.Http.HttpClientHandler>          _handlers;

		public Clients(ITable<HttpClient, System.Net.Http.HttpClientHandler> associated,
		               Func<Uri, System.Net.Http.HttpClientHandler> handlers)
		{
			_associated = associated;
			_handlers   = handlers;
		}

		public HttpClient Get(Uri parameter)
		{
			var handler = _handlers(parameter);
			var result  = new HttpClient(handler) {BaseAddress = parameter};
			_associated.Assign(result, handler);
			return result;
		}
	}
}