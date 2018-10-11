using System;

namespace Super.Model.Selection
{
	sealed class SelectedParameterSpecification<TFrom, TTo, TOut> : ISpecification<TFrom, TOut>
	{
		readonly ISpecification<TTo, TOut> _specification;
		readonly Func<TFrom, TTo>          _select;

		public SelectedParameterSpecification(Func<TFrom, TTo> select, ISpecification<TTo, TOut> specification)
		{
			_select        = @select;
			_specification = specification;
		}

		public bool IsSatisfiedBy(TFrom parameter) => _specification.IsSatisfiedBy(_select(parameter));

		public TOut Get(TFrom parameter) => _specification.Get(_select(parameter));
	}
}