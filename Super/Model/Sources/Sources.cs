using Super.Model.Selection;
using Super.Model.Selection.Stores;

namespace Super.Model.Sources
{
	sealed class ValueSelector<T> : Delegated<ISource<T>, T>
	{
		public static ValueSelector<T> Default { get; } = new ValueSelector<T>();

		ValueSelector() : base(x => x.Get()) {}
	}

	sealed class Sources<T> : Store<T, Source<T>>
	{
		public static Sources<T> Default { get; } = new Sources<T>();

		Sources() {}
	}
}