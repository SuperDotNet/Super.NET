using System.Reflection;
using Super.Model.Selection;
using Super.Reflection.Types;

namespace Super.Runtime.Objects
{
	sealed class InstanceMetadata<T> : Select<T, TypeInfo>
	{
		public static InstanceMetadata<T> Default { get; } = new InstanceMetadata<T>();

		InstanceMetadata() : base(InstanceType<T>.Default.Select(TypeMetadata.Default)) {}
	}
}