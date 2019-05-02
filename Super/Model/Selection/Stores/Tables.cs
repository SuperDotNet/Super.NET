using Super.Compose;
using Super.Model.Sequences;
using Super.Reflection.Types;
using Super.Runtime.Activation;
using System;
using System.Collections.Concurrent;

namespace Super.Model.Selection.Stores
{
	public sealed class Tables<TIn, TOut> : Select<Func<TIn, TOut>, ITable<TIn, TOut>>
	{
		public static Tables<TIn, TOut> Default { get; } = new Tables<TIn, TOut>();

		Tables() : base(Reflection.IsReference.Default.Get(Type<TIn>.Instance)
			                ? Start.A.Generic(typeof(ReferenceTables<,>))
			                       .Of.Type<ISelect<Func<TIn, TOut>, ITable<TIn, TOut>>>()
			                       .In(new Array<Type>(typeof(TIn), typeof(TOut)))
			                       .Assume()
			                       .Assume()
			                : Start.An.Instance(Activations<Func<TIn, TOut>, ConcurrentTables<TIn, TOut>>.Default)
			                       .Select(x => x.Get(new ConcurrentDictionary<TIn, TOut>()))) {}
	}
}