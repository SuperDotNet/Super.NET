using System;
using System.Reflection;

namespace Super.Model.Specifications
{
	public static class Objects
	{
		public static bool IsSatisfiedBy(this ISpecification<TypeInfo> @this, Type parameter)
			=> @this.IsSatisfiedBy(parameter.GetTypeInfo());
	}
}