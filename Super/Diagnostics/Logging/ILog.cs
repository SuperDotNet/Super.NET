using Serilog;
using Super.Model.Sources;

namespace Super.Diagnostics.Logging
{
	public interface ILog : ISource<ILogger>, ILogger {}
}