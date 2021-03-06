﻿using System;
using Super.Model.Results;
using Super.Runtime.Activation;

namespace Super.Model.Commands
{
	public class DelegatedInstanceCommand<T> : ICommand<T>, IActivateUsing<IResult<ICommand<T>>>
	{
		readonly Func<ICommand<T>> _instance;

		public DelegatedInstanceCommand(IResult<ICommand<T>> result) : this(result.Get) {}

		public DelegatedInstanceCommand(Func<ICommand<T>> instance) => _instance = instance;

		public void Execute(T parameter) => _instance().Execute(parameter);
	}
}