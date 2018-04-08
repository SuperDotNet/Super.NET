using Super.ExtensionMethods;
using System;
using Super.Model.Selection;

namespace Super.Model.Specifications
{
	public class SelectedParameterSpecification<TFrom, TTo> : ISpecification<TFrom>
	{
		readonly Func<TFrom, TTo> _select;
		readonly Func<TTo, bool>  _source;

		public SelectedParameterSpecification(ISpecification<TTo> source, ISelect<TFrom, TTo> select)
			: this(source.IsSatisfiedBy, @select.ToDelegate()) {}

		public SelectedParameterSpecification(Func<TTo, bool> source, Func<TFrom, TTo> select)
		{
			_select = @select;
			_source = source;
		}

		public bool IsSatisfiedBy(TFrom parameter) => _source(_select(parameter));
	}
}