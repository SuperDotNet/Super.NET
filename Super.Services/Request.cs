using Super.Model.Selection;
using System;
using System.Threading.Tasks;

namespace Super.Services
{
	sealed class Request<T> : DecoratedSelect<Task<T>, T>
	{
		public static Request<T> Default { get; } = new Request<T>();

		Request() : base(/*Retry<T>.Default*/null) {}
	}

	sealed class Request<TIn, TOut> : Select<TIn, Task<TOut>>
	{
		public Request(Func<TIn, Task<TOut>> source) : base(source) {}
	}
}