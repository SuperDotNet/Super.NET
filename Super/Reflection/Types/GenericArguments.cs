using Super.Model.Selection;
using Super.Model.Sequences;
using System;

namespace Super.Reflection.Types
{
	sealed class GenericArguments : Select<Type, Array<Type>>
	{
		public static GenericArguments Default { get; } = new GenericArguments();

		GenericArguments() : base(x => x.GenericTypeArguments) {}
	}
}