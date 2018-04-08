using System;
using System.Reflection;
using Super.Model.Sources;

namespace Super.Reflection
{
	class TypeMetadataValues<TAttribute, T> : MetadataValues<TypeInfo, TAttribute, T>
		where TAttribute : Attribute, ISource<T> {}
}