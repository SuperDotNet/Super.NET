using Super.Compose;
using Super.Model.Results;
using Super.Model.Selection;
using Super.Model.Selection.Alterations;
using Super.Reflection;
using Super.Runtime.Execution;

namespace Super.Runtime.Objects
{
	sealed class ResultAwareCast<TFrom, TTo> : Select<TFrom, TTo>
	{
		public static ResultAwareCast<TFrom, TTo> Default { get; } = new ResultAwareCast<TFrom, TTo>();

		ResultAwareCast() : base(Start.A.Selection<TFrom>()
		                              .AndOf<TTo>()
		                              .By.Cast.Unless(CanCast<TFrom, IResult<TTo>>.Default,
		                                              CastOrThrow<TFrom, IResult<TTo>>.Default.Then().Value().Get())) {}
	}

	sealed class OnlyOnceAlteration<TIn, TOut> : IAlteration<ISelect<TIn, TOut>>
	{
		public static OnlyOnceAlteration<TIn, TOut> Default { get; } = new OnlyOnceAlteration<TIn, TOut>();

		OnlyOnceAlteration() {}

		public ISelect<TIn, TOut> Get(ISelect<TIn, TOut> parameter)
			=> new First().Out()
			              .ToSelect(I.A<TIn>())
			              .To(parameter.If);
	}

	sealed class OncePerParameter<TIn, TOut> : IAlteration<ISelect<TIn, TOut>>
	{
		public static OncePerParameter<TIn, TOut> Default { get; } = new OncePerParameter<TIn, TOut>();

		OncePerParameter() {}

		public ISelect<TIn, TOut> Get(ISelect<TIn, TOut> parameter) => parameter.If(new First<TIn>());
	}
}