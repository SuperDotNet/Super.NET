﻿using Super.Model.Selection;
using System;
using System.Threading.Tasks;

namespace Super.Application.Services
{
	sealed class Request<T> : Select<Task<T>, T>
	{
		public static Request<T> Default { get; } = new Request<T>();

		Request() : base(x => x.Result) {}
	}

	sealed class Request<TIn, TOut> : Select<TIn, Task<TOut>>
	{
		public Request(Func<TIn, Task<TOut>> select) : base(select) {}
	}
}