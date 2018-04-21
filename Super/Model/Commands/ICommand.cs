using System.Reactive;

namespace Super.Model.Commands
{
	public interface ICommand : ICommand<Unit> {}

	public interface ICommand<in T>
	{
		void Execute(T parameter);
	}
}