using Super.Model.Selection;

namespace Super.Model.Sequences.Query
{
	public sealed class Yield<T> : Select<T, T[]>
	{
		public static Yield<T> Default { get; } = new Yield<T>();

		// TODO: Leased array:
		Yield() : base(x => new[] {x}) {}
	}
}