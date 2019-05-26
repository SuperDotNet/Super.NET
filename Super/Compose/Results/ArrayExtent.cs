using Super.Model.Results;
using Super.Runtime.Activation;

namespace Super.Compose.Results
{
	public sealed class ArrayExtent<T> : Extent<T[]>
	{
		public static ArrayExtent<T> Default { get; } = new ArrayExtent<T>();

		ArrayExtent() {}

		public IResult<T[]> New(uint size) => New<int, T[]>.Default.In((int)size);
	}
}