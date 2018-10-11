using System;

namespace Super.Model.Selection
{
	sealed class Selection<TParameter, TFrom, TTo> : ISelect<TParameter, TTo>
	{
		readonly Func<TFrom, TTo>        _select;
		readonly Func<TParameter, TFrom> _source;

		public Selection(Func<TParameter, TFrom> source, Func<TFrom, TTo> @select)
		{
			_select = @select;
			_source = source;
		}

		public TTo Get(TParameter parameter) => _select(_source(parameter));
	}

	sealed class ParameterSelection<_, TFrom, TTo> : ISelect<ISpecification<TTo, _>, ISpecification<TFrom, _>>
	{
		readonly Func<TFrom, TTo> _select;

		public ParameterSelection(Func<TFrom, TTo> select) => _select = @select;

		public ISelect<_, TTo> Get(ISelect<_, TFrom> parameter) => parameter.Select(_select);

		public ISpecification<TFrom, _> Get(ISpecification<TTo, _> parameter) => parameter.Select(_select);
	}
}