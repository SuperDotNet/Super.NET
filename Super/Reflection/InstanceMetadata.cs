using System;
using System.Reflection;
using Super.ExtensionMethods;
using Super.Model.Instances;
using Super.Model.Sources;
using Super.Runtime;

namespace Super.Reflection
{
	class InstanceMetadata<TAttribute, TParameter, TResult> : SpecificationSource<TParameter, TResult>
		where TAttribute : Attribute, IInstance<TResult>
	{
		public InstanceMetadata() : this(new TypeMetadataValue<TAttribute, TResult>()) {}

		public InstanceMetadata(ISpecification<TypeInfo, TResult> metadata)
			: this(metadata.ToStore().In(InstanceMetadataCoercer<TParameter>.Default)) {}

		public InstanceMetadata(ISpecification<TParameter, TResult> source) : base(source, source) {}
	}
}