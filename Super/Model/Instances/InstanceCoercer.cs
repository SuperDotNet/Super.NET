using Super.Model.Sources;

namespace Super.Model.Instances
{
	sealed class InstanceCoercer<T> : ISource<IInstance<T>, T>
	{
		public static InstanceCoercer<T> Default { get; } = new InstanceCoercer<T>();

		InstanceCoercer() {}

		public T Get(IInstance<T> parameter) => parameter.Get();
	}
}