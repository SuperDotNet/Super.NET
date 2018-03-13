using Super.Reflection;

namespace Super.Model.Collections
{
	sealed class SortMetadata<T> : InstanceMetadata<SortAttribute, T, int>
	{
		public static SortMetadata<T> Default { get; } = new SortMetadata<T>();

		SortMetadata() {}
	}
}