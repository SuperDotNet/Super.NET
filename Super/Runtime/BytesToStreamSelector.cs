using System.IO;
using Super.Model.Selection.Stores;

namespace Super.Runtime
{
	sealed class BytesToStreamSelector : ActivatedStore<byte[], MemoryStream>
	{
		public static BytesToStreamSelector Default { get; } = new BytesToStreamSelector();

		BytesToStreamSelector() {}
	}
}