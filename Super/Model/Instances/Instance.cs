using Super.ExtensionMethods;
using Super.Model.Sources;

namespace Super.Model.Instances
{
	public class Instance<T> : IInstance<T>
	{
		readonly T _instance;

		public Instance(T instance) => _instance = instance;

		public T Get() => _instance;

		public static implicit operator T(Instance<T> instance) => instance.Get();
	}

	public class Instance<TParameter, TResult> : DecoratedInstance<TResult>
	{
		public Instance(ISource<TParameter, TResult> source, TParameter parameter) :
			base(source.Fix(parameter).Singleton()) {}
	}
}