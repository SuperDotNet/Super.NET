using Super.ExtensionMethods;
using Super.Model.Selection;
using Super.Model.Selection.Alterations;
using System;

namespace Super.Text
{
	public sealed class NullOrEmptySelector : IAlteration<string>
	{
		public static NullOrEmptySelector Default { get; } = new NullOrEmptySelector();

		NullOrEmptySelector() {}

		public string Get(string parameter) => parameter ?? string.Empty;
	}

	class TextSelect<TParameter, TResult> : Select<string, TParameter, TResult>
	{
		public TextSelect(ISelect<string, Func<TParameter, TResult>> @select)
			: base(select.In(NullOrEmptySelector.Default).ToDelegate()) {}
	}
}
