﻿using System;
using Super.Runtime;

namespace Super.Text.Formatting
{
	public class FormatEntry<T> : Pairing<string, Func<T, string>>, IFormatEntry<T>
	{
		protected FormatEntry(string key, Func<T, string> value) : base(key, value) {}
	}
}