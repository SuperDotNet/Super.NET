using System;
using System.Reflection;
using Super.Model.Sources;

namespace Super.Reflection
{
	public sealed class TypeMetadataCoercer : ISource<Type, TypeInfo>
	{
		public static TypeMetadataCoercer Default { get; } = new TypeMetadataCoercer();

		TypeMetadataCoercer() {}

		public TypeInfo Get(Type parameter) => parameter.GetTypeInfo();
	}
}