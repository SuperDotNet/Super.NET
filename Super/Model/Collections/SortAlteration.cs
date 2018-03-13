using Super.Model.Sources;

namespace Super.Model.Collections
{
	sealed class SortAlteration<T> : OrderByAlteration<T, T>
	{
		public static SortAlteration<T> Default { get; } = new SortAlteration<T>();

		SortAlteration() : base(Self<T>.Default.Get, SortComparer<T>.Default) {}
	}
}