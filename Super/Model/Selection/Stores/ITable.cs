namespace Super.Model.Selection.Stores
{
	public interface ITable<TParameter, TResult> : ISpecification<TParameter, TResult>, IMutable<TParameter, TResult>
	{
		bool Remove(TParameter key);
	}
}