using Super.Model.Sources.Alterations;

namespace Super.Model.Sources
{
	public sealed class Self<T> : IAlteration<T>
	{
		public static IAlteration<T> Default { get; } = new Self<T>();

		Self() {}

		public T Get(T parameter) => parameter;
	}
}