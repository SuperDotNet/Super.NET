namespace Super.Model.Selection
{
	public class DecoratedSelect<TParameter, TResult> : Select<TParameter, TResult>
	{
		public DecoratedSelect(ISelect<TParameter, TResult> select) : base(select.ToDelegate()) {}
	}
}