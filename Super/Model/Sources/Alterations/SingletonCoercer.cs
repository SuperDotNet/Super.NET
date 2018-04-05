using Super.Model.Instances;

namespace Super.Model.Sources.Alterations
{
	public sealed class SingletonCoercer<T> : IAlteration<IInstance<T>>
	{
		public static SingletonCoercer<T> Default { get; } = new SingletonCoercer<T>();

		SingletonCoercer() {}

		public IInstance<T> Get(IInstance<T> parameter) => new DeferredSingleton<T>(parameter);
	}
}