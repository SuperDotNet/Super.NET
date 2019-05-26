using System;
using System.Threading.Tasks;
using Super.Model.Selection;

namespace Super.Services
{
	sealed class Request<T> : Select<Task<T>, T>
	{
		public static Request<T> Default { get; } = new Request<T>();

		Request() : base( /*Retry<T>.Default*/x => x.Result) {}
	}

	sealed class Request<TIn, TOut> : Select<TIn, Task<TOut>>
	{
		public Request(Func<TIn, Task<TOut>> select) : base(select) {}
	}
}