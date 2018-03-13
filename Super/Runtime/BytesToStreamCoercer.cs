using System.IO;
using Super.Model.Sources;

namespace Super.Runtime
{
	sealed class BytesToStreamCoercer : ISource<byte[], Stream>
	{
		public static BytesToStreamCoercer Default { get; } = new BytesToStreamCoercer();

		BytesToStreamCoercer() {}

		public Stream Get(byte[] parameter) => new MemoryStream(parameter);
	}
}