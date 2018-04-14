using System;
using System.Collections.Immutable;
using System.Reflection;
using Super.Model.Selection;
using Super.Model.Sources;
using Super.Model.Specifications;

namespace Super.Reflection
{
	class MetadataValues<TAttribute, T> : MetadataValues<MemberInfo, TAttribute, T>
		where TAttribute : Attribute, ISource<T> {}

	class MetadataValues<TMember, TAttribute, T> : Specification<TMember, ImmutableArray<T>>
		where TAttribute : Attribute, ISource<T>
		where TMember : MemberInfo
	{
		public MetadataValues() : this(IsDefined<TAttribute>.Default) {}

		public MetadataValues(ISpecification<TMember> specification)
			: base(specification,
			       new Specification<TMember, ImmutableArray<T>>(specification, Attributes<TAttribute, T>.Default)) {}
	}
}