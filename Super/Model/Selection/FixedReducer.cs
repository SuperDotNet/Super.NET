namespace Super.Model.Selection
{
	sealed class FixedReducer<TIn, TOut> : ISelect<ISelect<TIn, TOut>, TOut>
	{
		readonly TIn _parameter;

		public FixedReducer(TIn parameter) => _parameter = parameter;

		public TOut Get(ISelect<TIn, TOut> parameter) => parameter.Get(_parameter);
	}
}