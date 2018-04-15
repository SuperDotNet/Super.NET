using System;
using Super.ExtensionMethods;
using Super.Model.Selection;
using Super.Runtime.Activation;

namespace Super.Text.Formatting
{
	class Adapter<T> : IFormattable, IActivateMarker<T>
	{
		readonly Func<string, Func<T, string>> _selector;
		readonly T                             _subject;

		public Adapter(T subject, ISelect<T, string> format) : this(subject, format.ToDelegate().Accept) {}

		public Adapter(T subject, Func<string, Func<T, string>> selector)
		{
			_subject  = subject;
			_selector = selector;
		}

		public string ToString(string format, IFormatProvider formatProvider) => _selector(format)(_subject);
	}
}