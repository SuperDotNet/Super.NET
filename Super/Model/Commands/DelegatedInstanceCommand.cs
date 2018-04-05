using Super.ExtensionMethods;
using Super.Model.Instances;
using System;

namespace Super.Model.Commands
{
	public class DelegatedInstanceCommand<T> : ICommand<T>
	{
		readonly Func<ICommand<T>> _instance;

		public DelegatedInstanceCommand(IInstance<ICommand<T>> instance) : this(instance.ToDelegate()) {}

		public DelegatedInstanceCommand(Func<ICommand<T>> instance) => _instance = instance;

		public void Execute(T parameter) => _instance().Execute(parameter);
	}
}