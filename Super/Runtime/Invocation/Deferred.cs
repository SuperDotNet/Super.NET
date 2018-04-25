using Super.Model.Commands;
using Super.Model.Selection;
using Super.Model.Sources;
using Super.Runtime.Activation;

namespace Super.Runtime.Invocation
{
	class Deferred<TParameter, TResult> : DecoratedSelect<TParameter, TResult>,
	                                      IActivateMarker<ISelect<TParameter, TResult>>
	{
		public Deferred(ISelect<TParameter, TResult> select) : this(select, In<TParameter>.Default<TResult>().ToTable()) {}

		public Deferred(ISelect<TParameter, TResult> select, IMutable<TParameter, TResult> mutable)
			: base(mutable.Or(new Configuration<TParameter, TResult>(select, mutable))) {}
	}

	public class Deferred<T> : DecoratedSource<T>
	{
		public Deferred(ISource<T> source, IMutable<T> mutable) : this(source, mutable, mutable) {}

		public Deferred(ISource<T> source, ISource<T> store, ICommand<T> assign)
			: base(store.Or(source.Out(assign.ToConfiguration()))) {}
	}
}