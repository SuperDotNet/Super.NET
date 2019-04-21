namespace Super.Model.Selection
{
	public class DecoratedSelect<TIn, TOut> : Select<TIn, TOut>
	{
		public DecoratedSelect(ISelect<TIn, TOut> select) : base(select.Get) {}
	}
}