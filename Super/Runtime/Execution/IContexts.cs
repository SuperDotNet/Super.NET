using System.Reactive;
using Super.Model.Commands;
using Super.Model.Sources;

namespace Super.Runtime.Execution {
	public interface IContexts : ISource<object>, ICommand<object>, ICommand<Unit> {}
}