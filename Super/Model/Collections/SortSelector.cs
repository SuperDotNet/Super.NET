using Super.ExtensionMethods;
using Super.Model.Selection;
using Super.Model.Sources;

namespace Super.Model.Collections
{
	sealed class SortSelector<T> : DecoratedSelect<T, int>
	{
		public static SortSelector<T> Default { get; } = new SortSelector<T>();

		SortSelector() : base(In<T>.Select(CastOrValue<ISortAware>.Default)
		                           .Out(ValueSelector<int>.Default.Assigned())
		                           .Or(SortMetadata<T>.Default)
		                           .Or(In<T>.Result(-1))) {}
	}
}