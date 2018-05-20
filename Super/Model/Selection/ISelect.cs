namespace Super.Model.Selection
{
	public interface ISelect<in TParameter, out TResult>
	{
		TResult Get(TParameter parameter);
	}
}