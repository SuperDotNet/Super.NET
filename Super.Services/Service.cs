﻿using System;
using Super.Model.Selection;

namespace Super.Services
{
	public sealed class Service<T> : Select<Uri, T>
	{
		public static Service<T> Default { get; } = new Service<T>();

		Service() : base(ClientStore.Default.Select(Api<T>.Default)) {}
	}
}