namespace Super.Runtime.Environment
{
	sealed class PlatformAssemblyName : ExternalAssemblyName
	{
		public static PlatformAssemblyName Default { get; } = new PlatformAssemblyName();

		PlatformAssemblyName() : base("{0}.Platform") {}
	}
}