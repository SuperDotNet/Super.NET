using Super.Model.Selection;
using Super.Reflection;
using Super.Runtime;
using System;
using System.Collections.Generic;

namespace Super.Text
{
	public class TextSelect<TParameter, TResult> : Select<string, TParameter, TResult>
	{
		public TextSelect(ISelect<TParameter, TResult> @default,
		                  params KeyValuePair<string, Func<TParameter, TResult>>[] pairs)
			: this(@default.ToDelegate().Start().Out(I<string>.Default).Unless(Pairs.Select(pairs))) {}

		public TextSelect(ISelect<string, Func<TParameter, TResult>> select)
			: base(NullOrEmpty.Default.Select(select).Get) {}
	}
}