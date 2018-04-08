namespace Super.Model.Sources
{
	public interface ISource<out T>
	{
		T Get();
	}
}