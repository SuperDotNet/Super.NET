using Super.Model.Selection.Alterations;

namespace Super.Model.Selection
{
	public sealed class Self<T> : IAlteration<T>
	{
		public static IAlteration<T> Default { get; } = new Self<T>();

		Self() {}

		public T Get(T parameter) => parameter;
	}
}