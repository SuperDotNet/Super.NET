using Super.Model.Sources;

namespace Super.Model.Instances
{
	sealed class SourceCoercer<T> : ISource<IInstance<T>, T>
	{
		public static SourceCoercer<T> Default { get; } = new SourceCoercer<T>();

		SourceCoercer() {}

		public T Get(IInstance<T> parameter) => parameter.Get();
	}
}