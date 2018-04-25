using Super.Model.Selection;

namespace Super.Model.Collections
{
	class MetadataGroupName<T> : Specification<T, GroupName>, IGroupName<T>
	{
		public MetadataGroupName(ISpecification<string, GroupName> names)
			: this(DeclaredGroupNames<T>.Default, names) {}

		public MetadataGroupName(ISpecification<T, string> name, ISpecification<string, GroupName> names)
			: this(name, name.Out(names)) {}

		public MetadataGroupName(ISpecification<T, string> name, ISpecification<T, GroupName> names) :
			base(name.And(names), names) {}
	}
}