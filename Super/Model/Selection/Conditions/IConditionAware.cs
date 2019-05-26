namespace Super.Model.Selection.Conditions
{
	public interface IConditionAware<in T>
	{
		ICondition<T> Condition { get; }
	}
}