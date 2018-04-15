using System;
using System.Reflection;
using Super.Model.Selection;

namespace Super.Reflection.Types
{
	public sealed class TypeSelector : ISelect<TypeInfo, Type>
	{
		public static TypeSelector Default { get; } = new TypeSelector();

		TypeSelector() {}

		public Type Get(TypeInfo parameter) => parameter.AsType();
	}
}