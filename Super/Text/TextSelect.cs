using Super.Compose;
using Super.Model.Selection;
using Super.Runtime;
using System;

namespace Super.Text
{
	public class TextSelect<TIn, TOut> : Select<string, TIn, TOut>
	{
		public TextSelect(ISelect<TIn, TOut> @default, params Pair<string, Func<TIn, TOut>>[] pairs)
			: this(Start.A.Selection.Of<string>()
			            .By.Returning(@default.ToDelegate())
			            .Unless(Pairs.Select(pairs))) {}

		public TextSelect(ISelect<string, Func<TIn, TOut>> select) : base(NullOrEmpty.Default.Select(select).Get) {}
	}
}