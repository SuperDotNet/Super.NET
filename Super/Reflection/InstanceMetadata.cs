using Super.ExtensionMethods;
using Super.Model.Selection;
using Super.Model.Sources;
using Super.Runtime.Objects;
using System;
using System.Reflection;

namespace Super.Reflection
{
	class InstanceMetadata<TAttribute, TParameter, TResult> : Specification<TParameter, TResult>
		where TAttribute : Attribute, ISource<TResult>
	{
		public InstanceMetadata() : this(new TypeMetadataValue<TAttribute, TResult>()) {}

		public InstanceMetadata(ISpecification<TypeInfo, TResult> metadata)
			: this(metadata.ToStore().In(InstanceMetadataSelector<TParameter>.Default)) {}

		public InstanceMetadata(ISpecification<TParameter, TResult> source) : base(source, source) {}
	}
}