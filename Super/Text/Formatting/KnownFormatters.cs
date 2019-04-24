using Super.Compose;
using Super.Model.Selection;
using System;

namespace Super.Text.Formatting
{
	sealed class KnownFormatters : Select<object, IFormattable>
	{
		public static KnownFormatters Default { get; } = new KnownFormatters();

		KnownFormatters() : base(Start.A.Selection.Of.Any.By.StoredActivation<DefaultFormatter>()
		                              .Unless(ApplicationDomainFormatter.Default.Register())) {}
	}
}