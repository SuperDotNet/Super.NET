using System;
using System.Reflection;
using Super.Model.Selection;

namespace Super.Reflection.Types
{
	public sealed class TypeMetadata : ISelect<Type, TypeInfo>
	{
		public static TypeMetadata Default { get; } = new TypeMetadata();

		TypeMetadata() {}

		public TypeInfo Get(Type parameter) => parameter.GetTypeInfo();
	}
}