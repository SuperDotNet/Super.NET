using Serilog;
using Super.Model.Commands;
using Super.Runtime.Activation;

namespace Super.Diagnostics.Logging
{
	public interface ILogMessage<in T> : ICommand<T>, IActivateUsing<ILogger> {}
}