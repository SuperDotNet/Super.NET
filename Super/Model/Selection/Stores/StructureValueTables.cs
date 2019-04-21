using Super.Runtime.Activation;
using System;
using System.Runtime.CompilerServices;

namespace Super.Model.Selection.Stores
{
	public sealed class StructureValueTables<TIn, TOut>
		: ISelect<ConditionalWeakTable<TIn, Tuple<TOut>>, ITable<TIn, TOut>>,
		  IActivateUsing<Func<TIn, TOut>>
		where TIn : class
		where TOut : struct
	{
		public static StructureValueTables<TIn, TOut> Default { get; } = new StructureValueTables<TIn, TOut>();

		StructureValueTables() : this(Default<TIn, TOut>.Instance.Get) {}


		readonly Func<TIn, Tuple<TOut>> _source;

		public StructureValueTables(Func<TIn, TOut> source) => _source = new TupleSelector(source).Get;

		public ITable<TIn, TOut> Get(ConditionalWeakTable<TIn, Tuple<TOut>> parameter)
		{
			var get =
				new ConditionalWeakTableAccessAdapter<TIn, Tuple<TOut>>(parameter,
				                                                                  new ConditionalWeakTable<TIn,
					                                                                  Tuple<TOut>>.CreateValueCallback(_source));
			var access   = new StructureTableAccessAdapter<TIn, TOut>(get);
			var contains = new ConditionalWeakTableContainsAdapter<TIn, Tuple<TOut>>(parameter);
			var assign =
				new StructureTableAssignCommand<TIn, TOut>(new ConditionalWeakTableAssignCommand<TIn,
					                                                     Tuple<TOut>
				                                                     >(parameter));
			var result =
				new DelegatedTable<TIn, TOut>(contains.Get, assign.Execute, access.Get, parameter.Remove);
			return result;
		}

		sealed class TupleSelector : ISelect<TIn, Tuple<TOut>>
		{
			readonly Func<TIn, TOut> _source;

			public TupleSelector(Func<TIn, TOut> source) => _source = source;

			public Tuple<TOut> Get(TIn parameter) => Tuple.Create(_source(parameter));
		}
	}
}