﻿using Super.Model.Selection.Stores;
using System;

namespace Super.Model.Commands
{
	sealed class DelegateCommands<T> : ReferenceStore<Action<T>, ICommand<T>>
	{
		public static DelegateCommands<T> Default { get; } = new DelegateCommands<T>();

		DelegateCommands() : base(x => x.Target as ICommand<T> ?? new DelegatedCommand<T>(x)) {}
	}
}