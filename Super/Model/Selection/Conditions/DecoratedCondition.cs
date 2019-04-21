namespace Super.Model.Selection.Conditions
{
	public class DecoratedCondition<T> : DelegatedCondition<T>
	{
		public DecoratedCondition(ISelect<T, bool> condition) : base(condition.Get) {}
	}
}