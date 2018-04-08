using System;
using System.Reflection;
using Super.Model.Selection;
using Super.Model.Sources;
using Super.Model.Specifications;

namespace Super.Reflection
{
	class MetadataValue<TAttribute, T> : MetadataValue<MemberInfo, TAttribute, T>
		where TAttribute : Attribute, ISource<T> {}

	class MetadataValue<TMember, TAttribute, T> : Specification<TMember, T>
		where TAttribute : Attribute, ISource<T>
		where TMember : MemberInfo
	{
		public MetadataValue() : this(IsDefinedSpecification<TAttribute>.Default) {}

		public MetadataValue(ISpecification<TMember> specification)
			: base(specification, new DelegatedConditional<TMember, T>(specification, Attribute<TAttribute, T>.Default)) {}
	}
}