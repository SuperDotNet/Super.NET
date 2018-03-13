namespace Super.Model.Sources.Tables
{
	public interface ITable<TParameter, TResult> : ISpecification<TParameter, TResult>, IAssignable<TParameter, TResult>
	{
		bool Remove(TParameter key);
	}
}