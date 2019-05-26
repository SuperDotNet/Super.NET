using System;
using Super.Model.Selection;

namespace Super.Runtime.Objects
{
	sealed class Projectors : Select<Type, string, Func<object, IProjection>>, IProjectors
	{
		public static Projectors Default { get; } = new Projectors();

		Projectors() : base(KnownProjectors.Default.Select(x => x.Open().ToStore().ToDelegate()).Assume()) {}
	}
}