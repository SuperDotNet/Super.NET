using Super.Model.Results;
using Super.Runtime;
using Super.Runtime.Activation;
using System;

namespace Super.Model.Commands
{
	public class DelegatedInstanceCommand<T> : ICommand<T>, IActivateUsing<IResult<ICommand<T>>>
	{
		readonly Func<ICommand<T>> _instance;

		public DelegatedInstanceCommand(IResult<ICommand<T>> result) : this(result.Get) {}

		public DelegatedInstanceCommand(Func<ICommand<T>> instance) => _instance = instance;

		public void Execute(T parameter) => _instance().Execute(parameter);
	}

	sealed class SelectedAssignment<TIn, TOut> : IAssign<TIn, TOut>
	{
		readonly Func<TIn, ICommand<TOut>> _select;

		public SelectedAssignment(Func<TIn, ICommand<TOut>> select) => _select = select;

		public void Execute(Pair<TIn, TOut> parameter) => _select(parameter.Key).Execute(parameter.Value);
	}
}