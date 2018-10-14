using System;
using Super.Model.Specifications;

namespace Super.Model.Sequences.Query {
	public sealed class Only<T> : One<T>
	{
		public static Only<T> Default { get; } = new Only<T>();

		Only() : this(Always<T>.Default.IsSatisfiedBy) {}

		public Only(Func<T, bool> @where) : base(@where) {}
	}
}