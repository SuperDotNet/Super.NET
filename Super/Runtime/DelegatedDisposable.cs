using Super.Model.Collections;
using Super.Model.Commands;
using Super.Runtime.Activation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Super.Runtime
{
	sealed class EmptyDisposable : DelegatedDisposable
	{
		public static EmptyDisposable Default { get; } = new EmptyDisposable();

		EmptyDisposable() : base(() => {}) {}
	}

	public class Disposables : Membership<IDisposable>, IDisposable
	{
		readonly ICollection<IDisposable> _collection;

		public Disposables() : this(new Collection<IDisposable>()) {}

		public Disposables(ICollection<IDisposable> collection) : base(collection) => _collection = collection;

		public void Dispose()
		{
			foreach (var disposable in _collection)
			{
				disposable.Dispose();
			}
			_collection.Clear();
		}
	}

	sealed class DisposeCommand : DelegatedCommand<IDisposable>
	{
		public static ICommand<IDisposable> Default { get; } = new DisposeCommand();

		DisposeCommand() : base(x => x.Dispose()) {}
	}

	public class DelegatedDisposable : IDisposable, IActivateUsing<Action>
	{
		readonly Action _callback;

		public DelegatedDisposable(Action callback) => _callback = callback;

		public void Dispose() => _callback();
	}
}