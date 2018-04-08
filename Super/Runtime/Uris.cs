using Super.Model.Selection.Stores;
using System;

namespace Super.Runtime
{
	public sealed class Uris : ReferenceStore<string, Uri>
	{
		public static Uris Default { get; } = new Uris();

		Uris() {}
	}
}