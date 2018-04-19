using Super.Model.Selection;
using Super.Reflection.Types;
using System.Reflection;

namespace Super.Runtime.Objects
{
	sealed class InstanceMetadataSelector<T> : DecoratedSelect<T, TypeInfo>
	{
		public static InstanceMetadataSelector<T> Default { get; } = new InstanceMetadataSelector<T>();

		InstanceMetadataSelector() : base(InstanceTypeSelector<T>.Default.Out(TypeMetadataSelector.Default)) {}
	}
}