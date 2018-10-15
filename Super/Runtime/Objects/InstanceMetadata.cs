using Super.Model.Selection;
using Super.Reflection.Types;
using System.Reflection;

namespace Super.Runtime.Objects
{
	sealed class InstanceMetadata<T> : DecoratedSelect<T, TypeInfo>
	{
		public static InstanceMetadata<T> Default { get; } = new InstanceMetadata<T>();

		InstanceMetadata() : base(InstanceType<T>.Default.Select(TypeMetadata.Default)) {}
	}
}