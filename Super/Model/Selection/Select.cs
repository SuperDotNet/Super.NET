using Super.Runtime.Activation;
using System;
using System.Linq.Expressions;

namespace Super.Model.Selection
{
	public class Compile<TIn, TOut> : Select<TIn, TOut>
	{
		public Compile(Expression<Func<TIn, TOut>> select) : base(select.Compile()) {}
	}

	public class Select<TIn, TOut> : ISelect<TIn, TOut>,
	                                 IActivateUsing<Func<TIn, TOut>>
	{
		readonly Func<TIn, TOut> _source;

		public Select(ISelect<TIn, TOut> select) : this(select.Get) {}

		public Select(Func<TIn, TOut> select) => _source = select;

		public TOut Get(TIn parameter) => _source(parameter);
	}
}