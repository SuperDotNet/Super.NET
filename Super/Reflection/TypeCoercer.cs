using System;
using System.Reflection;
using Super.Model.Sources;

namespace Super.Reflection
{
	public sealed class TypeCoercer : ISource<TypeInfo, Type>
	{
		public static TypeCoercer Default { get; } = new TypeCoercer();

		TypeCoercer() {}

		public Type Get(TypeInfo parameter) => parameter.AsType();
	}
}