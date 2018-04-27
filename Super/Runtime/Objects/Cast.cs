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

		ValueAwareCast() : base(Cast<TFrom, TTo>.Default.Assigned(CanCast<TFrom, ISource<TTo>>.Default.If(Cast<TFrom, ISource<TTo>>.Default.Value()))) {}
	}

	sealed class OnlyOnceAlteration<TIn, TOut> : IAlteration<ISelect<TIn, TOut>>
	{
		public static OnlyOnceAlteration<TIn, TOut> Default { get; } = new OnlyOnceAlteration<TIn, TOut>();

		OnlyOnceAlteration() {}

		public ISelect<TIn, TOut> Get(ISelect<TIn, TOut> parameter) => new First().Out().Select(I<TIn>.Default).Out().If(parameter);
	}

	sealed class OncePerParameter<TIn, TOut> : IAlteration<ISelect<TIn, TOut>>
	{
		public static OncePerParameter<TIn, TOut> Default { get; } = new OncePerParameter<TIn, TOut>();

		OncePerParameter() {}

		public ISelect<TIn, TOut> Get(ISelect<TIn, TOut> parameter) => new First<TIn>().If(parameter);
	}
}