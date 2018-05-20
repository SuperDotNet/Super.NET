using System;
using Super.Model.Selection;

namespace Super.Runtime
{
	sealed class LocalFilePath : Select<Uri, string>
	{
		public static LocalFilePath Default { get; } = new LocalFilePath();

		LocalFilePath() : base(x => x.LocalPath) {}
	}
}