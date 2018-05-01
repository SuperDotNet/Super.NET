using Super.Model.Sources;
using System;
using System.Reactive;

namespace Super.Model.Commands
{
	public interface ICommand : ICommand<Unit> {}

	public interface ICommand<in T>
	{
		void Execute(T parameter);
	}

	public interface IDelegateAware<in T> : ISource<Action<T>> {}
}