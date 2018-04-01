using System;
using Super.Model.Sources;

namespace Super.Runtime
{
	public sealed class Authority : DelegatedSource<Uri, string>
	{
		public static Authority Default { get; } = new Authority();

		Authority() : base(x => x.GetLeftPart(UriPartial.Authority)) {}
	}
}