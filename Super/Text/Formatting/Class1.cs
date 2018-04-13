using JetBrains.Annotations;
using Super.ExtensionMethods;
using Super.Model.Selection;
using Super.Runtime;
using Super.Runtime.Activation;
using System;
using System.Globalization;

namespace Super.Text.Formatting
{
	sealed class Provider : ServiceProvider, IFormatProvider
	{
		public static Provider Default { get; } = new Provider();

		Provider() : base(CustomFormatter.Default, CultureInfo.CurrentCulture) {}

		public object GetFormat(Type formatType) => GetService(formatType);
	}

	sealed class CustomFormatter : ICustomFormatter
	{
		public static CustomFormatter Default { get; } = new CustomFormatter();

		CustomFormatter() : this(Formatters.Default) {}

		readonly ISelect<object, IFormattable> _select;

		public CustomFormatter(ISelect<object, IFormattable> table) => _select = table;

		public string Format(string format, object arg, IFormatProvider formatProvider)
			=> _select.Get(arg).ToString(format, formatProvider);
	}

	sealed class Formatters : Decorated<object, IFormattable>
	{
		public static Formatters Default { get; } = new Formatters();

		Formatters() : this(KnownFormatters.Default, Activations<object, DefaultFormatter>.Default) {}

		public Formatters(ISelect<object, IFormattable> formatters, ISelect<object, IFormattable> fallback)
			: base(formatters.Or(Cast<object>.Default.In<IFormattable>().ToSource()).Or(fallback)) {}
	}

	sealed class KnownFormatters : ISelect<object, IFormattable>
	{
		public static KnownFormatters Default { get; } = new KnownFormatters();

		KnownFormatters() : this(ApplicationDomainFormatter.Default.Register()) {}

		readonly ISelect<object, Func<object, IFormattable>> _source;

		public KnownFormatters(ISelect<object, Func<object, IFormattable>> source) => _source = source;

		public IFormattable Get(object parameter) => _source.Get(parameter)?.Invoke(parameter);
	}

	class Adapter<T> : IFormattable, IActivateMarker<T>
	{
		readonly T                             _subject;
		readonly Func<string, Func<T, string>> _selector;

		public Adapter(T subject, ISelect<T, string> format) : this(subject, format.ToDelegate().Accept) {}

		public Adapter(T subject, Func<string, Func<T, string>> selector)
		{
			_subject  = subject;
			_selector = selector;
		}

		public string ToString(string format, IFormatProvider formatProvider) => _selector(format)(_subject);
	}

	sealed class Formatters<T> : ISelect<T, IFormattable>,
	                             IActivateMarker<IFormatter<T>>,
	                             IActivateMarker<INamedFormatter<T>>
	{
		readonly Func<string, Func<T, string>> _select;

		[UsedImplicitly]
		public Formatters(IFormatter<T> formatter) : this(formatter.ToDelegate().Accept) {}

		[UsedImplicitly]
		public Formatters(INamedFormatter<T> formatter) : this(formatter.Get) {}

		public Formatters(Func<string, Func<T, string>> select) => _select = @select;

		public IFormattable Get(T parameter) => new Adapter<T>(parameter, _select);
	}

	sealed class DefaultApplicationDomainFormatter : IFormatter<AppDomain>
	{
		public static DefaultApplicationDomainFormatter Default { get; } = new DefaultApplicationDomainFormatter();

		DefaultApplicationDomainFormatter() {}

		public string Get(AppDomain parameter) => $"AppDomain: {parameter.FriendlyName}";
	}

	class Format<T> : Pair<string, Func<T, string>>
	{
		protected Format(string key, Func<T, string> value) : base(key, value) {}
	}

	/*sealed class DefaultFormat<T> : Format<T>, IActivateMarker<Func<T, string>>
	{
		public DefaultFormat(Func<T, string> value) : base(string.Empty, value) {}
	}*/

	sealed class ApplicationDomainName : Format<AppDomain>
	{
		public static ApplicationDomainName Default { get; } = new ApplicationDomainName();

		ApplicationDomainName() : base("F", x => x.FriendlyName) {}
	}

	sealed class ApplicationDomainIdentifier : Format<AppDomain>
	{
		public static ApplicationDomainIdentifier Default { get; } = new ApplicationDomainIdentifier();

		ApplicationDomainIdentifier() : base("I", x => x.Id.ToString()) {}
	}


	sealed class ApplicationDomainFormatter : TextSelect<AppDomain, string>, INamedFormatter<AppDomain>
	{
		public static ApplicationDomainFormatter Default { get; } = new ApplicationDomainFormatter();

		ApplicationDomainFormatter()
			: base(Pairs.From(ApplicationDomainName.Default,
			                  ApplicationDomainIdentifier.Default)
			            .Or(DefaultApplicationDomainFormatter.Default.AsDefault())) {}
	}

	public interface INamedFormatter<in T> : ISelect<string, Func<T, string>> {}

	sealed class DefaultFormatter : Adapter<object>
	{
		public DefaultFormatter(object subject) : base(subject, TextSelector<object>.Default) {}
	}
}