using System;
using System.Reflection;
using Super.Model.Instances;

namespace Super.Reflection
{
	class TypeMetadataValue<TAttribute, T> : MetadataValue<TypeInfo, TAttribute, T>
		where TAttribute : Attribute, IInstance<T> {}
}