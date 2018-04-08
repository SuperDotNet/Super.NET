using Super.Model.Selection;
using System;
using System.Reflection;

namespace Super.Reflection
{
	public sealed class TypeMetadataSelector : ISelect<Type, TypeInfo>
	{
		public static TypeMetadataSelector Default { get; } = new TypeMetadataSelector();

		TypeMetadataSelector() {}

		public TypeInfo Get(Type parameter) => parameter.GetTypeInfo();
	}
}