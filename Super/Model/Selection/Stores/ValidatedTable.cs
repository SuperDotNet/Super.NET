using Super.Model.Specifications;
using System.Collections.Generic;

namespace Super.Model.Selection.Stores
{
	public class ValidatedTable<TParameter, TResult> : ITable<TParameter, TResult>
	{
		readonly ISpecification<TParameter>  _specification;
		readonly ITable<TParameter, TResult> _table;

		public ValidatedTable(ISpecification<TParameter> specification, ITable<TParameter, TResult> table)
		{
			_specification = specification;
			_table         = table;
		}

		public bool IsSatisfiedBy(TParameter parameter)
			=> _specification.IsSatisfiedBy(parameter) && _table.IsSatisfiedBy(parameter);

		public TResult Get(TParameter parameter) => _specification.IsSatisfiedBy(parameter) ? _table.Get(parameter) : default;

		public void Execute(KeyValuePair<TParameter, TResult> parameter)
		{
			if (_specification.IsSatisfiedBy(parameter.Key))
			{
				_table.Execute(parameter);
			}
		}

		public bool Remove(TParameter key) => _specification.IsSatisfiedBy(key) && _table.Remove(key);
	}
}