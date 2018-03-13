using System;
using System.Collections.Immutable;
using System.Reflection;
using Super.Model.Instances;
using Super.Model.Sources;
using Super.Model.Specifications;

namespace Super.Reflection
{
	class MetadataValues<TAttribute, T> : MetadataValues<MemberInfo, TAttribute, T>
		where TAttribute : Attribute, IInstance<T> {}

	class MetadataValues<TMember, TAttribute, T> : SpecificationSource<TMember, ImmutableArray<T>>
		where TAttribute : Attribute, IInstance<T>
		where TMember : MemberInfo
	{
		public MetadataValues() : this(IsDefinedSpecification<TAttribute>.Default) {}

		public MetadataValues(ISpecification<TMember> specification)
			: base(specification,
			       new SpecificationSource<TMember, ImmutableArray<T>>(specification, Attributes<TAttribute, T>.Default)) {}
	}
}