using System;

namespace Super.Runtime.Environment
{
	[AttributeUsage(AttributeTargets.Assembly)]
	public sealed class PrimaryAssemblyAttribute : Attribute {}
}