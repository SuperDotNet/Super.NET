using Super.Testing.Objects;

namespace Super.Testing.Platform
{
	public sealed class Service<T> : IService<T>
	{
		public static Service<T> Default { get; } = new Service<T>();

		Service() {}

		public T Get(T parameter) => parameter;
	}
}