using Super.Model.Extents;
using Super.Model.Sources;
using Super.Model.Specifications;
using Super.Reflection.Types;
using System;
using System.Reflection;

namespace Super.Reflection
{
	sealed class IsContainedAttribute<T> : DecoratedSpecification<ICustomAttributeProvider> where T : Attribute
	{
		public static IsContainedAttribute<T> Default { get; } = new IsContainedAttribute<T>();

		IsContainedAttribute()
			: base(IsDefined<T>.Default
			                   .In()
			                   .And(IsAssignableFrom<ISource<T>>.Default.In().In(Type<T>.Metadata).Allow())
			                   .Cast(I<object>.Default)
			                   .Return()) {}
	}
}