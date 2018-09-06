using Super.Runtime.Environment;

namespace Super.Application.Hosting.xUnit
{
	public sealed class XunitTestingApplicationAttribute : HostingAttribute
	{
		public XunitTestingApplicationAttribute() : base(typeof(XunitTestingApplicationAttribute).Assembly) {}
	}
}
