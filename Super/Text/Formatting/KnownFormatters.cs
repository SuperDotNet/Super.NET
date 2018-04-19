using Super.Model.Selection;
using System;

namespace Super.Text.Formatting
{
	sealed class KnownFormatters : ISelect<object, IFormattable>
	{
		public static KnownFormatters Default { get; } = new KnownFormatters();

		KnownFormatters() : this(ApplicationDomainFormatter.Default.Register()) {}

		readonly ISelect<object, Func<object, IFormattable>> _source;

		public KnownFormatters(ISelect<object, Func<object, IFormattable>> source) => _source = source;

		public IFormattable Get(object parameter) => _source.Get(parameter)?.Invoke(parameter);
	}
}