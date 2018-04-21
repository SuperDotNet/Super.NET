using Super.Runtime;
using System;

namespace Super.Text.Formatting
{
	public interface IFormatEntry<T> : IPair<string, Func<T, string>> {}
}