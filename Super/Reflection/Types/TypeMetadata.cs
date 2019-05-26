using System;
using System.Reflection;
using Super.Model.Selection;

namespace Super.Reflection.Types
{
	public sealed class TypeMetadata : Select<Type, TypeInfo>
	{
		public static TypeMetadata Default { get; } = new TypeMetadata();

		TypeMetadata() : base(x => x.GetTypeInfo()) {}
	}
}