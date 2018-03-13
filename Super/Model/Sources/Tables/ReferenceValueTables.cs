using System;
using System.Runtime.CompilerServices;

namespace Super.Model.Sources.Tables
{
	public sealed class ReferenceValueTables<TParameter, TResult>
		: ISource<ConditionalWeakTable<TParameter, TResult>, ITable<TParameter, TResult>>
		where TParameter : class
		where TResult : class
	{
		public static ISource<Func<TParameter, TResult>, ReferenceValueTables<TParameter, TResult>> Defaults { get; }
			= new ReferenceValueTables<Func<TParameter, TResult>,
					ReferenceValueTables<TParameter, TResult>>(x => new ReferenceValueTables<TParameter, TResult>(x))
				.Get(new ConditionalWeakTable<Func<TParameter, TResult>, ReferenceValueTables<TParameter, TResult>>());

		readonly Func<TParameter, TResult> _source;

		public ReferenceValueTables(Func<TParameter, TResult> source) => _source = source;

		public ITable<TParameter, TResult> Get(ConditionalWeakTable<TParameter, TResult> parameter)
		{
			var get =
				new ConditionalWeakTableAccessAdapter<TParameter, TResult>(parameter,
				                                                           new ConditionalWeakTable<TParameter, TResult>.
					                                                           CreateValueCallback(_source));
			var contains = new ConditionalWeakTableContainsAdapter<TParameter, TResult>(parameter);
			var assign   = new ConditionalWeakTableAssignCommand<TParameter, TResult>(parameter);
			var result =
				new DelegatedTable<TParameter, TResult>(contains.IsSatisfiedBy, assign.Execute, get.Get, parameter.Remove);
			return result;
		}
	}
}