using System;
using Super.Model.Selection;

namespace Super.Runtime
{
	public sealed class Authority : Select<Uri, string>
	{
		public static Authority Default { get; } = new Authority();

		Authority() : base(x => x.GetLeftPart(UriPartial.Authority)) {}
	}
}