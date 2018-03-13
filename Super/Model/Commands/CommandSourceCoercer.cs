using System.Reactive;
using Super.ExtensionMethods;
using Super.Model.Sources;

namespace Super.Model.Commands
{
	sealed class CommandSourceCoercer<T> : ISource<ICommand<T>, ISource<T, Unit>>
	{
		public static CommandSourceCoercer<T> Default { get; } = new CommandSourceCoercer<T>();

		CommandSourceCoercer() {}

		public ISource<T, Unit> Get(ICommand<T> parameter) => parameter.Adapt();
	}
}