using Super.Compose;
using Super.Model.Selection;
using System;

namespace Super.Text.Formatting
{
	sealed class KnownFormatters : Select<object, IFormattable>
	{
		public static KnownFormatters Default { get; } = new KnownFormatters();

		KnownFormatters() : this(Start.A.Selection.Of.Any.By.StoredActivation<DefaultFormatter>()
		                              .Unless(Start.An.Instance<ApplicationDomainFormatter>().Register())) {}

		public KnownFormatters(ISelect<object, IFormattable> source) : base(source) {}
	}
}