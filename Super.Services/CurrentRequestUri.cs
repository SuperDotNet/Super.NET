﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Super.Model.Selection.Stores;
using System;

namespace Super.Services
{
	sealed class CurrentRequestUri : ReferenceStore<HttpRequest, Uri>
	{
		public static CurrentRequestUri Default { get; } = new CurrentRequestUri();

		CurrentRequestUri() : base(x => new Uri(x.GetDisplayUrl())) {}
	}
}