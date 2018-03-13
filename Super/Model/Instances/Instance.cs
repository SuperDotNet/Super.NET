namespace Super.Model.Instances
{
	public class Instance<T> : IInstance<T>
	{
		readonly T _instance;

		public Instance(T instance) => _instance = instance;

		public T Get() => _instance;
	}
}