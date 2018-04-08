using Super.Runtime;
using System;

namespace Super.Model.Selection
{
	sealed class DecorationSelector<TFrom, TTo, TResult> : ISelect<Decoration<TFrom, TResult>, Decoration<TTo, TResult>>
	{
		public static DecorationSelector<TFrom, TTo, TResult> Default { get; } =
			new DecorationSelector<TFrom, TTo, TResult>();

		DecorationSelector() : this(CastSelector<TFrom, TTo>.Default.Get) {}

		readonly Func<TFrom, TTo> _select;

		public DecorationSelector(Func<TFrom, TTo> coercer) => _select = coercer;

		public Decoration<TTo, TResult> Get(Decoration<TFrom, TResult> parameter)
			=> new Decoration<TTo, TResult>(_select(parameter.Parameter), parameter.Result);
	}
}