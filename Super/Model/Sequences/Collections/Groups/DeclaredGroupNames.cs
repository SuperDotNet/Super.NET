using Super.Reflection;

namespace Super.Model.Sequences.Collections.Groups
{
	sealed class DeclaredGroupNames<T> : InstanceMetadata<T, GroupElementAttribute, string>
	{
		public static DeclaredGroupNames<T> Default { get; } = new DeclaredGroupNames<T>();

		DeclaredGroupNames() {}
	}
}