using JetBrains.Annotations;
using Super.Compose;
using System;
using System.Runtime.CompilerServices;

namespace Super.Model.Selection.Stores
{
	public class AssociatedResource<TIn, TOut> : DecoratedTable<TIn, TOut>
	{
		public AssociatedResource() : this(Start.A.Selection<TIn>().AndOf<TOut>().By.Activation().Get) {}

		public AssociatedResource(Func<TIn, TOut> resource) : base(Tables<TIn, TOut>.Default.Get(resource)) {}
	}

	public class ReferenceValueTable<TIn, TOut> : DecoratedTable<TIn, TOut> where TIn : class
	                                                                        where TOut : class
	{
		[UsedImplicitly]
		public ReferenceValueTable() : this(_ => default) {}

		public ReferenceValueTable(Func<TIn, TOut> parameter)
			: base(new ReferenceValueTables<TIn, TOut>(parameter).Get(new ConditionalWeakTable<TIn, TOut>())) {}
	}
}