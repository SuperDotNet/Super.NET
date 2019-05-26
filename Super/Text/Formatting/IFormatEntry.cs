using System;
using Super.Runtime;

namespace Super.Text.Formatting
{
	public interface IFormatEntry<T> : IPair<string, Func<T, string>> {}
}