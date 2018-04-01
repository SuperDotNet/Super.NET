using Serilog;
using Super.Model.Sources.Alterations;

namespace Super.Diagnostics
{
	public sealed class ContextCoercer<T> : IAlteration<ILogger>
	{
		public static ContextCoercer<T> Default { get; } = new ContextCoercer<T>();

		ContextCoercer() {}

		public ILogger Get(ILogger parameter) => parameter.ForContext<T>();
	}
}