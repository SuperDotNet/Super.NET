using Super.Model.Selection;
using Super.Model.Sequences;
using System;
using System.Reflection;

namespace Super.Reflection.Types
{
	sealed class GenericParameters : Select<TypeInfo, Array<Type>>
	{
		public static GenericParameters Default { get; } = new GenericParameters();

		GenericParameters() : base(x => x.GenericTypeParameters) {}
	}
}