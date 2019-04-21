using System;
using System.Runtime.CompilerServices;

namespace Super.Model.Selection.Stores
{
	public sealed class ReferenceValueTables<TIn, TOut>
		: ISelect<ConditionalWeakTable<TIn, TOut>, ITable<TIn, TOut>>
		where TIn : class
		where TOut : class
	{
		public static ISelect<Func<TIn, TOut>, ReferenceValueTables<TIn, TOut>> Defaults { get; }
			= new ReferenceValueTables<Func<TIn, TOut>,
					ReferenceValueTables<TIn, TOut>>(x => new ReferenceValueTables<TIn, TOut>(x))
				.Get(new ConditionalWeakTable<Func<TIn, TOut>, ReferenceValueTables<TIn, TOut>>());

		public static ReferenceValueTables<TIn, TOut> Default { get; } = new ReferenceValueTables<TIn, TOut>();

		ReferenceValueTables() : this(Default<TIn, TOut>.Instance.Get) {}

		readonly Func<TIn, TOut> _source;

		public ReferenceValueTables(Func<TIn, TOut> source) => _source = source;

		public ITable<TIn, TOut> Get(ConditionalWeakTable<TIn, TOut> parameter)
		{
			var get =
				new ConditionalWeakTableAccessAdapter<TIn, TOut>(parameter,
				                                                           new ConditionalWeakTable<TIn, TOut>.
					                                                           CreateValueCallback(_source));
			var contains = new ConditionalWeakTableContainsAdapter<TIn, TOut>(parameter);
			var assign   = new ConditionalWeakTableAssignCommand<TIn, TOut>(parameter);
			var result =
				new DelegatedTable<TIn, TOut>(contains.Get, assign.Execute, get.Get, parameter.Remove);
			return result;
		}
	}
}