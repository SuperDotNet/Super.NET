using JetBrains.Annotations;
using Super.Model.Selection;
using Super.Runtime.Activation;
using System;

namespace Super.Text.Formatting
{
	sealed class Formatters : DecoratedSelect<object, IFormattable>
	{
		public static Formatters Default { get; } = new Formatters();

		Formatters() : this(KnownFormatters.Default, MarkedActivations<object, DefaultFormatter>.Default) {}

		public Formatters(ISelect<object, IFormattable> formatters, ISelect<object, IFormattable> fallback)
			: base(formatters.Assigned(In<object>.Cast<IFormattable>()).Assigned(fallback)) {}
	}

	sealed class Formatters<T> : ISelect<T, IFormattable>,
	                             IActivateMarker<IFormatter<T>>,
	                             IActivateMarker<ISelectFormatter<T>>
	{
		readonly Func<string, Func<T, string>> _select;

		[UsedImplicitly]
		public Formatters(IFormatter<T> formatter) : this(formatter.ToDelegate().Accept) {}

		[UsedImplicitly]
		public Formatters(ISelectFormatter<T> formatter) : this(formatter.Get) {}

		public Formatters(Func<string, Func<T, string>> select) => _select = select;

		public IFormattable Get(T parameter) => new Adapter<T>(parameter, _select);
	}
}