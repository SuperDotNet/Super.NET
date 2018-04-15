using Serilog.Events;
using Super.Model.Selection;

namespace Super.Diagnostics.Logging
{
	public interface IScalars : ISelect<LogEvent, IScalar> {}
}