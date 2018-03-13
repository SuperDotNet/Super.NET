namespace Super.Model.Instances
{
	public interface IInstance<out T>
	{
		T Get();
	}
}