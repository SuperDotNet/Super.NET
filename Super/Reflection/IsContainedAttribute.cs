using Super.Model.Sources;
using Super.Model.Specifications;
using Super.Reflection.Types;
using System;
using System.Reflection;

namespace Super.Reflection
{
	sealed class IsContainedAttribute<TAttribute, T> : DecoratedSpecification<ICustomAttributeProvider> where TAttribute : Attribute
	{
		public static IsContainedAttribute<TAttribute, T> Default { get; } = new IsContainedAttribute<TAttribute, T>();

		IsContainedAttribute()
			: base(IsDefined<TAttribute>.Default.And(IsAssignableFrom<ISource<T>>.Default.Out().Out(Type<TAttribute>.Metadata))) {}
	}
}