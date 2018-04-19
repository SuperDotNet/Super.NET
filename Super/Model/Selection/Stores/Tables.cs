using Super.Model.Specifications;
using Super.Reflection.Types;
using Super.Runtime.Activation;
using System;
using System.Collections.Concurrent;

namespace Super.Model.Selection.Stores
{
	public sealed class Tables<TParameter, TResult>
		: Select<Func<TParameter, TResult>, ITable<TParameter, TResult>>
	{
		public static Tables<TParameter, TResult> Default { get; } = new Tables<TParameter, TResult>();

		Tables() : base(IsValueType.Default.IsSatisfiedBy(typeof(TParameter))
			                ? Activations<Func<TParameter, TResult>, ConcurrentTables<TParameter, TResult>>
			                  .Default
			                  .Out(Activation<ConcurrentDictionary<TParameter, TResult>>.Default.Get)
			                  .ToDelegate()
			                : new Generic<ISelect<Func<TParameter, TResult>, ITable<TParameter, TResult>>>(typeof(ReferenceTables<,>))
			                  .Get(typeof(TParameter), typeof(TResult))()
			                  .ToDelegate()) {}
	}
}