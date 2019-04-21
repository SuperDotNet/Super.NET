using Super.Compose;
using Super.Model.Sequences;
using Super.Reflection;
using Super.Reflection.Types;
using Super.Runtime.Activation;
using System;

namespace Super.Model.Selection.Stores
{
	public sealed class Tables<TIn, TOut> : DecoratedSelect<Func<TIn, TOut>, ITable<TIn, TOut>>
	{
		public static Tables<TIn, TOut> Default { get; } = new Tables<TIn, TOut>();

		Tables() : this(Start.A.Generic(typeof(ReferenceTables<,>))
		                     .Of.Type<ISelect<Func<TIn, TOut>, ITable<TIn, TOut>>>()
		                     .In(new Array<Type>(typeof(TIn), typeof(TOut)))
		                     .Emit()
		                     .Emit(),
		                Start.An.Instance<Activations<Func<TIn, TOut>, ConcurrentTables<TIn, TOut>>>()
		                     .Select(x => x.New().Get())) {}

		public Tables(ISelect<Func<TIn, TOut>, ITable<TIn, TOut>> references,
		              ISelect<Func<TIn, TOut>, ITable<TIn, TOut>> structures)
			: base(IsReference.Default.Get(Type<TIn>.Instance) ? references : structures) {}
	}
}