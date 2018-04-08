using Serilog;
using Super.Model.Selection.Alterations;

namespace Super.Diagnostics
{
	public sealed class ContextSelector<T> : IAlteration<ILogger>
	{
		public static ContextSelector<T> Default { get; } = new ContextSelector<T>();

		ContextSelector() {}

		public ILogger Get(ILogger parameter) => parameter.ForContext<T>();
	}
}