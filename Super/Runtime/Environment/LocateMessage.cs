using Super.Text;
using System;

namespace Super.Runtime.Environment
{
	sealed class LocateMessage : Message<Type>
	{
		public static LocateMessage Default { get; } = new LocateMessage();

		LocateMessage() :
			base(x => $"Could not locate an external/environmental type for {x}.  Please ensure there is a primary assembly registered with the PrimaryAssemblyAttribute, and that there is a corresponding assembly either named <PrimaryAssemblyName>.Environment for environmental-specific components or <PrimaryAssemblyName>.Platform for platform-specific components. Please also ensure that the component libraries contains one public type that implements the requested type.") {}
	}
}