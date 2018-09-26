using Super.Model.Sequences;
using System.Linq;
using Xunit;

namespace Super.Testing.Application.Model.Sequences
{
	public class ArrayPartitionsTests
	{
		[Fact]
		void Verify()
		{
			ArrayPartitions<int>.Default.Get(Enumerable.Range(0, 10_000).ToArray());
		}
	}
}