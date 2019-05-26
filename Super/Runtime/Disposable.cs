using JetBrains.Annotations;
using Super.Model.Commands;
using Super.Runtime.Activation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Super.Model.Sequences.Collections;

namespace Super.Runtime
{
	sealed class EmptyDisposable : Disposable
	{
		public static EmptyDisposable Default { get; } = new EmptyDisposable();

		EmptyDisposable() : base(() => {}) {}
	}

	public class Disposables : Membership<IDisposable>, IDisposable
	{
		readonly IList<IDisposable> _collection;

		[UsedImplicitly]
		public Disposables() : this(new Collection<IDisposable>()) {}

		public Disposables(IList<IDisposable> collection) : base(collection) => _collection = collection;

		public void Dispose()
		{
			var count = _collection.Count;
			for (var i = 0; i < count; i++)
			{
				_collection[i].Dispose();
			}
			_collection.Clear();
		}
	}

	sealed class DisposeCommand : Command<IDisposable>
	{
		public static ICommand<IDisposable> Default { get; } = new DisposeCommand();

		DisposeCommand() : base(x => x.Dispose()) {}
	}

	public class Disposable : IDisposable, IActivateUsing<Action>
	{
		readonly Action _callback;

		public Disposable(Action callback) => _callback = callback;

		public void Dispose() => _callback();
	}
}