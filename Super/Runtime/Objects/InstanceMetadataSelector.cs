using System.Reflection;
using Super.ExtensionMethods;
using Super.Model.Selection;
using Super.Reflection;

namespace Super.Runtime.Objects
{
	sealed class InstanceMetadataSelector<T> : Decorated<T, TypeInfo>
	{
		public static InstanceMetadataSelector<T> Default { get; } = new InstanceMetadataSelector<T>();

		InstanceMetadataSelector() : base(InstanceTypeSelector<T>.Default.Out(TypeMetadataSelector.Default)) {}
	}
}