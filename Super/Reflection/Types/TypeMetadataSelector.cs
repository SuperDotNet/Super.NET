using System;
using System.Reflection;
using Super.Model.Selection;

namespace Super.Reflection.Types
{
	public sealed class TypeMetadataSelector : ISelect<Type, TypeInfo>
	{
		public static TypeMetadataSelector Default { get; } = new TypeMetadataSelector();

		TypeMetadataSelector() {}

		public TypeInfo Get(Type parameter) => parameter.GetTypeInfo();
	}
}