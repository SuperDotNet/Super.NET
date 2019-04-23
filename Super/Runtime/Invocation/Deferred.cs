using JetBrains.Annotations;
using Super.Compose;
using Super.Model.Commands;
using Super.Model.Results;
using Super.Model.Selection;
using Super.Model.Selection.Stores;
using Super.Runtime.Activation;

namespace Super.Runtime.Invocation
{
	class Deferred<TIn, TOut> : Select<TIn, TOut>, IActivateUsing<ISelect<TIn, TOut>>
	{
		[UsedImplicitly]
		public Deferred(ISelect<TIn, TOut> select) : this(select, Start.A.Selection<TIn>()
		                                                               .AndOf<TOut>()
		                                                               .Into.Table()) {}

		public Deferred(ISelect<TIn, TOut> select, ITable<TIn, TOut> assignable)
			: this(@select, assignable, assignable) {}

		public Deferred(ISelect<TIn, TOut> select, IAssign<TIn, TOut> assign, ISelect<TIn, TOut> source)
			: base(Start.A.Selection(new Configuration<TIn, TOut>(select, assign)).Unless(source)) {}
	}

	public class Deferred<T> : Result<T>
	{
		public Deferred(IResult<T> result, IMutable<T> mutable) : this(result, mutable, mutable) {}

		public Deferred(IResult<T> result, IResult<T> store, ICommand<T> assign)
			: base(result.Select(assign.Then().ToConfiguration().Get())
			             .Unless(store)) {}
	}
}