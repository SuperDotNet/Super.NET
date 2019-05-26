using System;
using Super.Model.Selection;
using Super.Model.Selection.Conditions;
using Super.Reflection.Types;

namespace Super.Text.Formatting
{
	public class Formatter : Conditional<object, IFormattable>, IFormatter
	{
		public Formatter(IConditional<object, IFormattable> condition)
			: this(condition.Condition, condition) {}

		public Formatter(ICondition<object> condition, ISelect<object, IFormattable> source)
			: base(condition, source) {}
	}

	public sealed class Formatter<T> : Formatter
	{
		public Formatter(ISelect<object, IFormattable> condition) : base(IsOf<T>.Default, condition) {}
	}
}