using Super.Model.Selection;
using Super.Runtime.Activation;
using System;

namespace Super.Text.Formatting
{
	class Adapter<T> : IFormattable, IActivateUsing<T>
	{
		readonly Func<string, Func<T, string>> _selector;
		readonly T                             _subject;

		public Adapter(T subject, ISelect<T, string> format) : this(subject, format.ToDelegate().Accept) {}

		public Adapter(T subject, Func<string, Func<T, string>> selector)
		{
			_subject  = subject;
			_selector = selector;
		}

		public string ToString(string format, IFormatProvider formatProvider)
		{
			var selector = _selector(format);
			var s = selector(_subject);
			return s;
		}
	}
}