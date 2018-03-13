using Super.Model.Sources;

namespace Super.Runtime
{
	sealed class UnassignedValueCoercer<T> : ISource<T?, T> where T : struct
	{
		public static UnassignedValueCoercer<T> Default { get; } = new UnassignedValueCoercer<T>();

		UnassignedValueCoercer() {}

		public T Get(T? parameter) => parameter.GetValueOrDefault();
	}
}