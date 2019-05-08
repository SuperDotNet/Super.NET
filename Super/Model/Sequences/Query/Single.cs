using System;
using Super.Model.Selection.Conditions;

namespace Super.Model.Sequences.Query
{
	public sealed class Single<T> : One<T>
	{
		public static Single<T> Default { get; } = new Single<T>();

		Single() : this(Always<T>.Default.Get) {}

		public Single(Func<T, bool> where) : base(where, () => throw new InvalidOperationException()) {}
	}
}