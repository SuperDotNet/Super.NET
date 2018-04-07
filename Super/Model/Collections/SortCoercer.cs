using Super.ExtensionMethods;
using Super.Model.Containers;
using Super.Model.Instances;
using Super.Model.Sources;

namespace Super.Model.Collections
{
	sealed class SortCoercer<T> : DecoratedSource<T, int>
	{
		public static SortCoercer<T> Default { get; } = new SortCoercer<T>();

		SortCoercer() : base(In<T>.Select(Container<ISortAware>.Default)
		                          .Out(InstanceValueCoercer<int>.Default.Assigned())
		                          .Or(SortMetadata<T>.Default)
		                          .Or(In<T>.New(-1))) {}
	}
}