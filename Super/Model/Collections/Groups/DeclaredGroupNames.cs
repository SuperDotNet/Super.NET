using Super.Reflection;

namespace Super.Model.Collections.Groups
{
	sealed class DeclaredGroupNames<T> : InstanceMetadata<GroupElementAttribute, T, string>
	{
		public static DeclaredGroupNames<T> Default { get; } = new DeclaredGroupNames<T>();

		DeclaredGroupNames() {}
	}
}