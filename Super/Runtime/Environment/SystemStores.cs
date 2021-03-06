using System;
using Super.Compose;
using Super.Model.Results;
using Super.Runtime.Invocation;

namespace Super.Runtime.Environment
{
	public static class SystemStores
	{
		public static IResult<T> New<T>(Func<T> create) => new Deferred<T>(create, New<T>());

		public static IStore<T> New<T>() => Implementations<T>.Store.Get();
	}

	sealed class SystemStores<T> : Result<IMutable<T>>
	{
		public static SystemStores<T> Default { get; } = new SystemStores<T>();

		SystemStores() : base(Implementations.Activator.In(A.Type<T>())
		                                     .Then()
		                                     .Cast<IMutable<T>>()
		                                     .Selector()) {}
	}
}