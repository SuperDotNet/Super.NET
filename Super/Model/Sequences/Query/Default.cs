using System.Buffers;

namespace Super.Model.Sequences.Query
{
	sealed class Default
	{
		public static ArrayPool<int> Numbers { get; } = ArrayPool<int>.Shared;
	}
}