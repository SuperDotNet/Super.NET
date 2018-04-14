using Super.ExtensionMethods;
using Super.Model.Selection;
using Super.Runtime.Objects;
using System;
using System.Reflection;

namespace Super.Reflection
{
	class InstanceMetadata<TAttribute, TParameter, TResult> : Specification<TParameter, TResult> where TAttribute : Attribute
	{
		public InstanceMetadata() : this(ContainedAttribute<TAttribute, TResult>.Default) {}

		public InstanceMetadata(ISpecification<ICustomAttributeProvider, TResult> metadata)
			: this(metadata.In(InstanceMetadataSelector<TParameter>.Default)) {}

		public InstanceMetadata(ISpecification<TParameter, TResult> source) : base(source, source) {}
	}
}