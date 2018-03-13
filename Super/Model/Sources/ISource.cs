namespace Super.Model.Sources
{
	public interface ISource<in TParameter, out TResult>
	{
		TResult Get(TParameter parameter);
	}
}