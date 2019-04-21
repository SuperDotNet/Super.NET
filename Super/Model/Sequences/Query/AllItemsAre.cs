using Super.Runtime.Activation;
using Super.Runtime.Invocation;
using System;
using Super.Model.Selection.Conditions;

namespace Super.Model.Sequences.Query
{
	public sealed class AllItemsAre<T> : DelegatedCondition<T[]>, IActivateUsing<Func<T, bool>>
	{
		public AllItemsAre(Func<T, bool> specification) : this(new Predicate<T>(specification)) {}

		public AllItemsAre(Predicate<T> specification)
			: base(new Invocation0<T[], Predicate<T>, bool>(Array.TrueForAll, specification).Get) {}
	}
}