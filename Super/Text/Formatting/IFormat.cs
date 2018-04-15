using System;
using Super.Runtime;

namespace Super.Text.Formatting
{
	public interface IFormat<T> : IPair<string, Func<T, string>> {}
}