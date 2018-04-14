using Super.Text;

namespace Super.Runtime.Environment
{
	sealed class PrimaryAssemblyMessage : Message<object>
	{
		public static PrimaryAssemblyMessage Default { get; } = new PrimaryAssemblyMessage();

		PrimaryAssemblyMessage() :
			base(x => $"A request was made for this application's primary assembly, but one could not be located.  Please ensure the entry or primary assembly or executable used for this application is marked with the PrimaryAssemblyAttribute.") {}
	}
}