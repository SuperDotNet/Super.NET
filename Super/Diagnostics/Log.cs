using Serilog;
using Super.ExtensionMethods;
using Super.Model.Instances;

namespace Super.Diagnostics
{
	public sealed class Log<T> : DeferredInstance<ILogger>
	{
		public static ILogger Default { get; } = new Log<T>().Get();

		Log() : base(Logger.Default.Adapt(ContextCoercer<T>.Default)) {}
	}
}