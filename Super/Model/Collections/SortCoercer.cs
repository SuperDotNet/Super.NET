using Super.ExtensionMethods;
using Super.Model.Sources;
using Super.Reflection;

namespace Super.Model.Collections
{
	sealed class SortCoercer<T> : DecoratedSource<T, int>
	{
		public static SortCoercer<T> Default { get; } = new SortCoercer<T>();

		SortCoercer() : base(Assume<T>.Default(-1)
		                              .Unless(SortMetadata<T>.Default)
		                              .Unless(I<ISortAware>.Default)) {}
	}
}