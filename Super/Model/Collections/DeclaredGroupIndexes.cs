using Super.Reflection;

namespace Super.Model.Collections
{
	sealed class DeclaredGroupIndexes<T> : InstanceMetadata<InsertGroupElementAttribute, T, int>
	{
		public static DeclaredGroupIndexes<T> Default { get; } = new DeclaredGroupIndexes<T>();

		DeclaredGroupIndexes() {}
	}
}