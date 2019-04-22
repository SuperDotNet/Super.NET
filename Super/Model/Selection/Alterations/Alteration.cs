using Super.Model.Selection.Conditions;
using System;

namespace Super.Model.Selection.Alterations
{
	public class Alteration<T> : Select<T, T>, IAlteration<T>
	{
		public Alteration(ISelect<T, T> @select) : this(@select.Get) {}

		public Alteration(Func<T, T> alteration) : base(alteration) {}
	}

	public class ValidatedAlteration<T> : Validated<T, T>
	{
		public ValidatedAlteration(ICondition<T> condition, ISelect<T, T> @select) : base(condition, @select) {}

		public ValidatedAlteration(ICondition<T> condition, ISelect<T, T> @select, ISelect<T, T> fallback) : base(condition, @select, fallback) {}

		public ValidatedAlteration(Func<T, bool> specification, Func<T, T> source) : base(specification, source) {}

		public ValidatedAlteration(Func<T, bool> specification, Func<T, T> source, Func<T, T> fallback) : base(specification, source, fallback) {}
	}
}