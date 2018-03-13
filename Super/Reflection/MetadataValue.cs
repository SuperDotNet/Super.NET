using System;
using System.Reflection;
using Super.Model.Instances;
using Super.Model.Sources;
using Super.Model.Specifications;

namespace Super.Reflection
{
	class MetadataValue<TAttribute, T> : MetadataValue<MemberInfo, TAttribute, T>
		where TAttribute : Attribute, IInstance<T> {}

	class MetadataValue<TMember, TAttribute, T> : SpecificationSource<TMember, T>
		where TAttribute : Attribute, IInstance<T>
		where TMember : MemberInfo
	{
		public MetadataValue() : this(IsDefinedSpecification<TAttribute>.Default) {}

		public MetadataValue(ISpecification<TMember> specification)
			: base(specification, new Conditional<TMember, T>(specification, Attribute<TAttribute, T>.Default)) {}
	}
}