using Super.Runtime.Activation;
using System;

namespace Super.Model.Sequences
{
	sealed class Repeat<T> : IArray<uint, T>
	{
		public static Repeat<T> Default { get; } = new Repeat<T>();

		Repeat() : this(New<T>.Default.ToDelegateReference()) {}

		readonly IStore<T> _store;
		readonly Func<T>   _create;

		public Repeat(Func<T> create) : this(Allocated<T>.Default, create) {}

		public Repeat(IStore<T> store, Func<T> create)
		{
			_store  = store;
			_create = create;
		}

		public Array<T> Get(uint parameter)
		{
			var result = _store.Get(parameter).Instance;
			for (var i = 0; i < parameter; i++)
			{
				result[i] = _create();
			}

			return result;
		}
	}
}