using Super.Model.Specifications;
using System;

namespace Super.Model.Selection.Alterations
{
	public class DelegatedAlteration<T> : Select<T, T>, IAlteration<T>
	{
		public DelegatedAlteration(Func<T, T> alteration) : base(alteration) {}
	}

	public class ValidatedAlteration<T> : Validated<T, T>
	{
		public ValidatedAlteration(ISpecification<T> specification, ISelect<T, T> @select) : base(specification, @select) {}

		public ValidatedAlteration(ISpecification<T> specification, ISelect<T, T> @select, ISelect<T, T> fallback) : base(specification, @select, fallback) {}

		public ValidatedAlteration(Func<T, bool> specification, Func<T, T> source) : base(specification, source) {}

		public ValidatedAlteration(Func<T, bool> specification, Func<T, T> source, Func<T, T> fallback) : base(specification, source, fallback) {}
	}
}