using System;

namespace Super.Model.Selection
{
	public interface ISelect<in TParameter, out TResult>
	{
		TResult Get(TParameter parameter);
	}

	public interface IEnhancedSelect<TIn, out TOut> where TIn : struct
	{
		TOut Get(in TIn parameter);
	}

	public interface ISelect<in TParameter, in TIn, out TOut> : ISelect<TParameter, Func<TIn, TOut>> {}

	public class Select<TParameter, TIn, TOut> : Select<TParameter, Func<TIn, TOut>>, ISelect<TParameter, TIn, TOut>
	{
		public Select(Func<TParameter, Func<TIn, TOut>> select) : base(select) {}
	}

}