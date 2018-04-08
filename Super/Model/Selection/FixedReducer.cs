namespace Super.Model.Selection
{
	sealed class FixedReducer<TParameter, TResult> : ISelect<ISelect<TParameter, TResult>, TResult>
	{
		readonly TParameter _parameter;

		public FixedReducer(TParameter parameter) => _parameter = parameter;

		public TResult Get(ISelect<TParameter, TResult> parameter) => parameter.Get(_parameter);
	}
}