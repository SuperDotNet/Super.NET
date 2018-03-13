using System;
using System.Runtime.CompilerServices;
using Super.Runtime.Activation;

namespace Super.Model.Sources.Tables
{
	public sealed class StructureValueTables<TParameter, TResult>
		: ISource<ConditionalWeakTable<TParameter, Tuple<TResult>>, ITable<TParameter, TResult>>,
		  IActivateMarker<Func<TParameter, TResult>>
		where TParameter : class
		where TResult : struct
	{
		readonly Func<TParameter, Tuple<TResult>> _source;

		public StructureValueTables(Func<TParameter, TResult> source) => _source = new TupleSource(source).Get;

		public ITable<TParameter, TResult> Get(ConditionalWeakTable<TParameter, Tuple<TResult>> parameter)
		{
			var get =
				new ConditionalWeakTableAccessAdapter<TParameter, Tuple<TResult>>(parameter,
				                                                                  new ConditionalWeakTable<TParameter,
					                                                                  Tuple<TResult>>.CreateValueCallback(_source));
			var access   = new StructureTableAccessAdapter<TParameter, TResult>(get);
			var contains = new ConditionalWeakTableContainsAdapter<TParameter, Tuple<TResult>>(parameter);
			var assign =
				new StructureTableAssignCommand<TParameter, TResult>(new ConditionalWeakTableAssignCommand<TParameter,
					                                                     Tuple<TResult>
				                                                     >(parameter));
			var result =
				new DelegatedTable<TParameter, TResult>(contains.IsSatisfiedBy, assign.Execute, access.Get, parameter.Remove);
			return result;
		}

		sealed class TupleSource : ISource<TParameter, Tuple<TResult>>
		{
			readonly Func<TParameter, TResult> _source;

			public TupleSource(Func<TParameter, TResult> source) => _source = source;

			public Tuple<TResult> Get(TParameter parameter) => Tuple.Create(_source(parameter));
		}
	}
}