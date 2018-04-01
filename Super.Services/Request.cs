using System;
using Super.Model.Sources;

namespace Super.Services
{
	sealed class Request<T> : DecoratedSource<IObservable<T>, T>
	{
		public static Request<T> Default { get; } = new Request<T>();

		Request() : base(Retry<T>.Default) {}
	}

	sealed class Request<TParameter, TResult> : DelegatedSource<TParameter, IObservable<TResult>>
	{
		public Request(Func<TParameter, IObservable<TResult>> source) : base(source) {}
	}
}