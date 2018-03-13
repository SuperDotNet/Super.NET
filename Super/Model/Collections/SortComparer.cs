namespace Super.Model.Collections
{
	sealed class SortComparer<T> : DelegatedComparer<T>
	{
		public static SortComparer<T> Default { get; } = new SortComparer<T>();

		SortComparer() : base(SortCoercer<T>.Default.Get) {}
	}
}