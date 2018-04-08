using System;
using System.Reflection;
using Super.Model.Sources;

namespace Super.Reflection
{
	class TypeMetadataValue<TAttribute, T> : MetadataValue<TypeInfo, TAttribute, T>
		where TAttribute : Attribute, ISource<T> {}
}