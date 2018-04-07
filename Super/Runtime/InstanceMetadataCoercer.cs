using System;
using System.Reflection;
using Super.Model.Sources;

namespace Super.Runtime
{
	sealed class InstanceMetadataCoercer<T> : SelectedParameterSource<Type, T, TypeInfo>
	{
		public static InstanceMetadataCoercer<T> Default { get; } = new InstanceMetadataCoercer<T>();

		InstanceMetadataCoercer() : base(x => x.GetTypeInfo(), InstanceTypeCoercer<T>.Default.Get) {}
	}
}