using Super.Model.Selection;
using System;

namespace Super.Text.Formatting
{
	sealed class KnownFormatters : DecoratedSelect<object, IFormattable>
	{
		public static KnownFormatters Default { get; } = new KnownFormatters();

		KnownFormatters() : this(Start.From<object>()
		                              .Activate<DefaultFormatter>()
		                              .Unless(ApplicationDomainFormatter.Default.Register())) {}

		public KnownFormatters(ISelect<object, IFormattable> source) : base(source) {}
	}
}