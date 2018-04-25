using Super.Model.Extents;
using Super.Model.Specifications;
using Super.Reflection.Types;
using Super.Runtime.Activation;
using System;

namespace Super.Model.Selection.Stores
{
	public sealed class Tables<TParameter, TResult> : Select<Func<TParameter, TResult>, ITable<TParameter, TResult>>
	{
		public static Tables<TParameter, TResult> Default { get; } = new Tables<TParameter, TResult>();

		Tables()
			: this(IsValueType.Default.IsSatisfiedBy(typeof(TParameter))
				       ? MarkedActivations<Func<TParameter, TResult>, ConcurrentTables<TParameter, TResult>>
				         .Default
				         .Out(x => x.Fold())
				       : new Generic<ISelect<Func<TParameter, TResult>, ITable<TParameter, TResult>>>(typeof(ReferenceTables<,>))
					       .Get(typeof(TParameter), typeof(TResult))()) {}

		public Tables(ISelect<Func<TParameter, TResult>, ITable<TParameter, TResult>> select) : base(select.ToDelegate()) {}
	}
}