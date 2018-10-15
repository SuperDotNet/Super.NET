using Super.Model.Selection;
using Super.Reflection;

namespace Super.Model.Collections
{
	sealed class SortSelector<T> : DecoratedSelect<T, int>
	{
		public static SortSelector<T> Default { get; } = new SortSelector<T>();

		SortSelector() : base((-1).Start()
		                          .Out(I<T>.Default)
		                          .Unless(SortMetadata<T>.Default)
		                          .UnlessCast(Self<ISortAware>.Default.Value())) {}
	}
}