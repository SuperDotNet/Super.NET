using Super.Model.Selection;
using Super.Reflection.Types;

namespace Super.Model.Collections
{
	sealed class SortSelector<T> : DecoratedSelect<T, int>
	{
		public static SortSelector<T> Default { get; } = new SortSelector<T>();

		SortSelector() : base(In<T>.Start(-1)
		                           .Unless(SortMetadata<T>.Default)
		                           .Unless(IsType<T, ISortAware>.Default,
		                                   In<T>.CastForValue<ISortAware>().Value())) {}
	}
}