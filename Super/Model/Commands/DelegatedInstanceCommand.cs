using Super.Model.Sources;
using Super.Runtime.Activation;
using System;
using System.Collections.Generic;

namespace Super.Model.Commands
{
	public class DelegatedInstanceCommand<T> : ICommand<T>, IActivateMarker<ISource<ICommand<T>>>
	{
		readonly Func<ICommand<T>> _instance;

		public DelegatedInstanceCommand(ISource<ICommand<T>> source) : this(source.Get) {}

		public DelegatedInstanceCommand(Func<ICommand<T>> instance) => _instance = instance;

		public void Execute(T parameter) => _instance().Execute(parameter);
	}

	sealed class SelectedAssignment<TParameter, TResult> : IAssignable<TParameter, TResult>
	{
		readonly Func<TParameter, ICommand<TResult>> _select;

		public SelectedAssignment(Func<TParameter, ICommand<TResult>> select) => _select = @select;

		public void Execute(KeyValuePair<TParameter, TResult> parameter)
			=> _select(parameter.Key).Execute(parameter.Value);
	}
}