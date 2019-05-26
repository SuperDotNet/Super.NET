using Super.Runtime.Environment;

namespace Super.Application.Hosting.AzureFunctions
{
	public sealed class AzureFunctionsApplicationAttribute : HostingAttribute
	{
		public AzureFunctionsApplicationAttribute() : base(typeof(AzureFunctionsApplicationAttribute).Assembly) {}
	}
}