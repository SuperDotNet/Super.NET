using Super.Compose;
using Super.Model.Selection.Stores;
using System;

namespace Super.Runtime
{
	public sealed class Uris : ReferenceValueStore<string, Uri>
	{
		public static Uris Default { get; } = new Uris();

		Uris() : base(Start.A.Selection<string>().AndOf<Uri>().By.Instantiation.Get) {}
	}
}