using Super.Model.Results;
using Super.Runtime;
using System;

namespace Super.Model.Commands
{
	public interface ICommand : ICommand<None> {}

	public interface ICommand<in T>
	{
		void Execute(T parameter);
	}

	public interface IDelegateAware<in T> : IResult<Action<T>> {}
}