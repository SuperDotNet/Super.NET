using System;
using System.Reflection;
using Super.Model.Selection;
using Super.Model.Sequences;

namespace Super.Reflection.Types
{
	sealed class GenericParameters : Select<TypeInfo, Array<Type>>
	{
		public static GenericParameters Default { get; } = new GenericParameters();

		GenericParameters() : base(x => x.GenericTypeParameters) {}
	}

	/*sealed class GenericArguments : Select<Type, Array<Type>>
	{
		public static GenericArguments Default { get; } = new GenericArguments();

		GenericArguments() : base(x => x.GetGenericArguments()) {}
	}*/
}