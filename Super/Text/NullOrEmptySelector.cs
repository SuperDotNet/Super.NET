using Super.ExtensionMethods;
using Super.Model.Selection;
using Super.Model.Selection.Alterations;
using Super.Runtime;
using System;
using System.Collections.Generic;

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
		public TextSelect(ISelect<TParameter, TResult> @default, params KeyValuePair<string, Func<TParameter, TResult>>[] pairs)
			: this(Pairs.Select(pairs).Or(@default.AsDefault())) {}

		public TextSelect(ISelect<string, Func<TParameter, TResult>> @select)
			: base(select.In(NullOrEmptySelector.Default).ToDelegate()) {}
	}
}
