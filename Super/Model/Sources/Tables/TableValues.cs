using System.Collections.Generic;
using Super.Model.Collections;
using Super.Model.Specifications;

namespace Super.Model.Sources.Tables
{
	public class TableValues<TParameter, TResult> : ValueSource<TParameter, TResult>, ISpecification<TParameter, TResult>
	{
		readonly ISpecification<TParameter> _source;

		public TableValues(IDictionary<TParameter, TResult> store)
			: this(new Table<TParameter, TResult>(store), new Values<TParameter, TResult>(store)) {}

		public TableValues(ITable<TParameter, TResult> source, IEnumerable<TResult> items)
			: base(source, items) => _source = source;

		public bool IsSatisfiedBy(TParameter parameter) => _source.IsSatisfiedBy(parameter);
	}
}