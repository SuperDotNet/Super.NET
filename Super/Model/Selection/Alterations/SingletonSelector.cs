using Super.Model.Results;

namespace Super.Model.Selection.Alterations
{
	public sealed class SingletonSelector<T> : IAlteration<IResult<T>>
	{
		public static SingletonSelector<T> Default { get; } = new SingletonSelector<T>();

		SingletonSelector() {}

		public IResult<T> Get(IResult<T> parameter) => new DeferredSingleton<T>(parameter.Get);
	}
}