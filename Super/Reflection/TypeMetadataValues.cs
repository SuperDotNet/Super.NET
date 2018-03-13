using System;
using System.Reflection;
using Super.Model.Instances;

namespace Super.Reflection
{
	class TypeMetadataValues<TAttribute, T> : MetadataValues<TypeInfo, TAttribute, T>
		where TAttribute : Attribute, IInstance<T> {}
}