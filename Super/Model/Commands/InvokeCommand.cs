﻿using System;
using Super.Runtime;

namespace Super.Model.Commands
{
	sealed class InvokeCommand<T> : ICommand<None>
	{
		readonly Func<T> _delegate;

		public InvokeCommand(Func<T> @delegate) => _delegate = @delegate;

		public void Execute(None _)
		{
			_delegate();
		}
	}
}