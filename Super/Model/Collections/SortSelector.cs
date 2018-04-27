using Super.Model.Selection;

namespace Super.Model.Collections
{
	sealed class SortSelector<T> : DecoratedSelect<T, int>
	{
		public static SortSelector<T> Default { get; } = new SortSelector<T>();

		SortSelector() : base(In<T>.Start(-1)
		                           .Unless(SortMetadata<T>.Default)
		                           .Unless(In<ISortAware>.Select(x => x.Get()))) {}
	}
}