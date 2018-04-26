using Super.Model.Selection;
using Super.Model.Selection.Alterations;
using Super.Model.Sources;
using Super.Reflection;
using Super.Runtime.Execution;

namespace Super.Runtime.Objects
{
	sealed class Cast<TFrom, TTo> : DecoratedSelect<TFrom, TTo>
	{
		public static Cast<TFrom, TTo> Default { get; } = new Cast<TFrom, TTo>();

		Cast() : base(CanCast<TFrom, TTo>.Default
		                                 .If(CastSelector<TFrom, TTo>.Default, Default<TFrom, TTo>.Instance)) {}
	}

	sealed class ValueAwareCast<TFrom, TTo> : DecoratedSelect<TFrom, TTo>
	{
		public static ValueAwareCast<TFrom, TTo> Default { get; } = new ValueAwareCast<TFrom, TTo>();

		ValueAwareCast() : base(Cast<TFrom, TTo>.Default.Or(Cast<TFrom, ISource<TTo>>.Default.Out(x => x.Value().Assigned()))) {}
	}

	/*sealed class OnlyOnceAlteration<T> : IAlteration<ISelect<T, bool>>
	{
		public static OnlyOnceAlteration<T> Default { get; } = new OnlyOnceAlteration<T>();

		OnlyOnceAlteration() {}

		public ISelect<T, bool> Get(ISelect<T, bool> parameter) => OnlyOnceAlteration<T, bool>.Default.Get(parameter).And(parameter);
	}*/

	sealed class OnlyOnceAlteration<TIn, TOut> : IAlteration<ISelect<TIn, TOut>>
	{
		public static OnlyOnceAlteration<TIn, TOut> Default { get; } = new OnlyOnceAlteration<TIn, TOut>();

		OnlyOnceAlteration() {}

		public ISelect<TIn, TOut> Get(ISelect<TIn, TOut> parameter) => parameter.If(new First().Enter().Allow(I<TIn>.Default).Exit());
	}

	/*sealed class OncePerParameter<T> : IAlteration<ISelect<T, bool>>
	{
		public static OncePerParameter<T> Default { get; } = new OncePerParameter<T>();

		OncePerParameter() {}

		public ISelect<T, bool> Get(ISelect<T, bool> parameter) => OncePerParameter<T, bool>.Default.Get(parameter).And(parameter);
	}*/

	sealed class OncePerParameter<TIn, TOut> : IAlteration<ISelect<TIn, TOut>>
	{
		public static OncePerParameter<TIn, TOut> Default { get; } = new OncePerParameter<TIn, TOut>();

		OncePerParameter() {}

		public ISelect<TIn, TOut> Get(ISelect<TIn, TOut> parameter) => parameter.If(new First<TIn>());
	}
}