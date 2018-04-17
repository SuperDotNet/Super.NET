using Super.ExtensionMethods;
using Super.Model.Selection;

namespace Super.Model.Collections
{
	public readonly struct GroupName
	{
		public GroupName(string name) => Name = name;

		public string Name { get; }

		public bool Equals(GroupName other) => string.Equals(Name, other.Name);

		public override bool Equals(object obj) => !ReferenceEquals(null, obj) && obj is GroupName phase && Equals(phase);

		public override int GetHashCode() => Name != null ? Name.GetHashCode() : 0;
	}

	sealed class GroupName<T> : DecoratedSelect<T, GroupName>, IGroupName<T>
	{
		public GroupName(GroupName defaultName, ISpecification<string, GroupName> names)
			: base(new MetadataGroupName<T>(names).Or(In<T>.Result(defaultName))) {}
	}
}