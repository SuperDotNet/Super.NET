using Super.Reflection;

namespace Super.Model.Collections
{
	sealed class SortMetadata<T> : InstanceMetadata<T, SortAttribute, int>
	{
		public static SortMetadata<T> Default { get; } = new SortMetadata<T>();

		SortMetadata() {}
	}
}