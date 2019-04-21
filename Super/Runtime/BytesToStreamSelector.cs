using Super.Model.Selection.Stores;
using System.IO;

namespace Super.Runtime
{
	sealed class BytesToStreamSelector : ActivatedStore<byte[], MemoryStream>
	{
		public static BytesToStreamSelector Default { get; } = new BytesToStreamSelector();

		BytesToStreamSelector() {}
	}
}