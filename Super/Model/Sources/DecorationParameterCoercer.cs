using System;
using Super.Runtime;

namespace Super.Model.Sources
{
	sealed class DecorationParameterCoercer<TFrom, TTo, TResult>
		: ISource<Decoration<TFrom, TResult>, Decoration<TTo, TResult>>
	{
		public static DecorationParameterCoercer<TFrom, TTo, TResult> Default { get; } =
			new DecorationParameterCoercer<TFrom, TTo, TResult>();

		DecorationParameterCoercer() : this(CastCoercer<TFrom, TTo>.Default.Get) {}

		readonly Func<TFrom, TTo> _coercer;

		public DecorationParameterCoercer(Func<TFrom, TTo> coercer) => _coercer = coercer;

		public Decoration<TTo, TResult> Get(Decoration<TFrom, TResult> parameter)
			=> new Decoration<TTo, TResult>(_coercer.Invoke(parameter.Parameter), parameter.Result);
	}
}