using Super.Model.Selection;
using Super.Runtime;
using System;
using System.Collections.Generic;

namespace Super.Text
{
	public class TextSelect<TParameter, TResult> : Select<string, TParameter, TResult>
	{
		public TextSelect(ISelect<TParameter, TResult> @default,
		                  params KeyValuePair<string, Func<TParameter, TResult>>[] pairs)
			: this(Pairs.Select(pairs).Or(@default.AsDefault())) {}

		public TextSelect(ISelect<string, Func<TParameter, TResult>> select)
			: base(NullOrEmptySelector.Default.Out(select).Get) {}
	}
}