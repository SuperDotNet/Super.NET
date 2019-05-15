using Super.Model.Selection;

namespace Super.Model.Sequences
{
	public interface IProject<T> : IProject<T, T> {}

	/*class Project<T> : Select<ArrayView<T>, ArrayView<T>>, IProject<T>
	{
		public Project(ISelect<ArrayView<T>, ArrayView<T>> select) : base(select.Get) {}
	}*/

	public interface IProject<TIn, TOut> : ISelect<ArrayView<TIn>, ArrayView<TOut>> {}

	public readonly struct Selection
	{
		public static Selection Default { get; } = new Selection(0);

		public static implicit operator Selection(Assigned<uint> length) => new Selection(0, length);

		public Selection(uint start, Assigned<uint> length = default)
		{
			Start  = start;
			Length = length;
		}

		public uint Start { get; }

		public Assigned<uint> Length { get; }

		public static bool operator ==(Selection left, Selection right) => left.Equals(right);

		public static bool operator !=(Selection left, Selection right) => !left.Equals(right);

		bool Equals(Selection other) => Start == other.Start && Length == other.Length;

		public override bool Equals(object obj)
			=> !ReferenceEquals(null, obj) && obj is Selection other && Equals(other);

		public override int GetHashCode()
		{
			unchecked
			{
				return ((int)Start * 397) ^ Length.GetHashCode();
			}
		}
	}
}