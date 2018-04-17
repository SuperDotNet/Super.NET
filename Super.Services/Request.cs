using System;
using Super.Model.Selection;

namespace Super.Services
{
	sealed class Request<T> : DecoratedSelect<IObservable<T>, T>
	{
		public static Request<T> Default { get; } = new Request<T>();

		Request() : base(Retry<T>.Default) {}
	}

	sealed class Request<TParameter, TResult> : Select<TParameter, IObservable<TResult>>
	{
		public Request(Func<TParameter, IObservable<TResult>> source) : base(source) {}
	}
}