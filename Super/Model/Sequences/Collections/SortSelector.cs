using Super.Compose;
using Super.Model.Selection;

namespace Super.Model.Sequences.Collections
{
	sealed class SortSelector<T> : Select<T, int>
	{
		public static SortSelector<T> Default { get; } = new SortSelector<T>();

		SortSelector() : base(Start.A.Selection.Of<T>()
		                           .By.Returning(-1)
		                           .Unless(SortMetadata<T>.Default)
		                           .UnlessIsOf(Start.A.Selection<ISortAware>()
		                                            .By.Self.AsDefined()
		                                            .Then()
		                                            .Value()
		                                            .Get())) {}
	}
}