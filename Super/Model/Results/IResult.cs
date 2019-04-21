namespace Super.Model.Results
{
	public interface IResult<out T>
	{
		T Get();
	}
}