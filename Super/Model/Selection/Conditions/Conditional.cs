using Super.Compose;
using System;

namespace Super.Model.Selection.Conditions
{
	public interface IConditionAware<in T>
	{
		ICondition<T> Condition { get; }
	}

	public interface IConditional<in TIn, out TOut> : IConditionAware<TIn>, ISelect<TIn, TOut> {}

	public class ConditionAware<T> : IConditionAware<T>
	{
		public ConditionAware(ICondition<T> condition) => Condition = condition;

		public ICondition<T> Condition { get; }
	}

	sealed class ParameterSelection<_, TFrom, TTo> : ISelect<IConditional<TTo, _>, IConditional<TFrom, _>>
	{
		readonly Func<TFrom, TTo> _select;

		public ParameterSelection(Func<TFrom, TTo> select) => _select = @select;

		public IConditional<TFrom, _> Get(IConditional<TTo, _> parameter)
			=> new SelectedConditional<TFrom, TTo, _>(parameter, _select);
	}

	public class Conditional<TIn, TOut> : Validated<TIn, TOut>, IConditional<TIn, TOut>
	{
		public Conditional(ICondition<TIn> condition, IConditional<TIn, TOut> source)
			: this(source.Condition.Then().Or(condition), source.Get) {}

		public Conditional(Func<TIn, bool> condition, Func<TIn, TOut> source)
			: this(condition.Target as ICondition<TIn> ?? new DelegatedCondition<TIn>(condition), source) {}

		public Conditional(ICondition<TIn> condition, Func<TIn, TOut> source)
			: this(condition, source, Start.A.Selection<TIn>().By.Default<TOut>().Get) {}

		public Conditional(ICondition<TIn> condition, Func<TIn, TOut> source, Func<TIn, TOut> fallback)
			: base(condition.Get, source, fallback) => Condition = condition;

		public ICondition<TIn> Condition { get; }
	}
}