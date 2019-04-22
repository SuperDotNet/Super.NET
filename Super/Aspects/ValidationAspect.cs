using Super.Model.Selection;
using Super.Model.Selection.Alterations;
using System;

namespace Super.Aspects
{
	public interface IAspects<TIn, TOut> : IAlteration<ISelect<TIn, TOut>> {}

	class ValidationAspect<TIn, TOut> : IAspects<TIn, TOut>
	{
		readonly Action<TIn> _validate;

		public ValidationAspect(Action<TIn> validate) => _validate = validate;

		public ISelect<TIn, TOut> Get(ISelect<TIn, TOut> parameter)
			=> new Validator<TIn, TOut>(parameter.Get, _validate);
	}

	public sealed class Validator<TIn, TOut> : ISelect<TIn, TOut>
	{
		readonly Func<TIn, TOut> _select;
		readonly Action<TIn>     _validate;

		public Validator(Func<TIn, TOut> select, Action<TIn> validate)
		{
			_select   = @select;
			_validate = validate;
		}

		public TOut Get(TIn parameter)
		{
			_validate(parameter);
			return _select(parameter);
		}
	}
}