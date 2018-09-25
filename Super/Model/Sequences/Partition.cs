using Super.Model.Collections;

namespace Super.Model.Sequences
{
	public struct Partition<T>
	{
		public Partition(ArrayView<T> view, Store<T> current)
		{
			View = view;
			Current = current;
		}

		public ArrayView<T> View { get; }

		public Store<T> Current { get; }
	}
}
