using System;
using Super.Runtime;

namespace Super.Text.Formatting
{
	public class Format<T> : Pair<string, Func<T, string>>, IFormat<T>
	{
		protected Format(string key, Func<T, string> value) : base(key, value) {}
	}
}