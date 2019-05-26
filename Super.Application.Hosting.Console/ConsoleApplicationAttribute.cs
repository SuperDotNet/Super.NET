using Super.Runtime.Environment;

namespace Super.Application.Hosting.Console
{
	public sealed class ConsoleApplicationAttribute : HostingAttribute
	{
		public ConsoleApplicationAttribute() : base(typeof(ConsoleApplicationAttribute).Assembly) {}
	}
}