using Super.Model.Selection;
using System;
using System.Reflection;

namespace Super.Reflection
{
	public sealed class TypeSelector : ISelect<TypeInfo, Type>
	{
		public static TypeSelector Default { get; } = new TypeSelector();

		TypeSelector() {}

		public Type Get(TypeInfo parameter) => parameter.AsType();
	}
}