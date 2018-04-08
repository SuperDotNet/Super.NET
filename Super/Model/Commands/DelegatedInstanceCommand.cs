using Super.ExtensionMethods;
using Super.Runtime.Activation;
using System;
using Super.Model.Sources;

namespace Super.Model.Commands
{
	public class DelegatedInstanceCommand<T> : ICommand<T>, IActivateMarker<ISource<ICommand<T>>>
	{
		readonly Func<ICommand<T>> _instance;

		public DelegatedInstanceCommand(ISource<ICommand<T>> source) : this(source.ToDelegate()) {}

		public DelegatedInstanceCommand(Func<ICommand<T>> instance) => _instance = instance;

		public void Execute(T parameter) => _instance().Execute(parameter);
	}
}