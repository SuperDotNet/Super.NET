using Super.Model.Sources;

namespace Super.Model.Selection.Alterations
{
	public sealed class SingletonSelector<T> : IAlteration<ISource<T>>
	{
		public static SingletonSelector<T> Default { get; } = new SingletonSelector<T>();

		SingletonSelector() {}

		public ISource<T> Get(ISource<T> parameter) => new DeferredSingleton<T>(parameter);
	}
}