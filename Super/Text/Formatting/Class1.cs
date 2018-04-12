using Super.ExtensionMethods;
using Super.Model.Selection;
using Super.Runtime.Activation;
using System;
using System.Collections.Generic;
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

		readonly ISpecification<Type, Func<object, IFormattable>> _source;

		public KnownFormatters(params KeyValuePair<Type, Func<object, IFormattable>>[] registrations)
			: this(registrations.ToDictionary().ToTable()) {}

		public KnownFormatters(ISpecification<Type, Func<object, IFormattable>> source) => _source = source;

		public IFormattable Get(object parameter)
			=> _source.IsSatisfiedBy(parameter.GetType()) ? _source.Get(parameter.GetType())(parameter) : null;
	}

	class Formatter<T> : IFormattable, IActivateMarker<T>
	{
		readonly T                             _subject;
		readonly Func<string, Func<T, string>> _selector;

		public Formatter(T subject, ISelect<T, string> format) : this(subject, format.ToDelegate().Accept) {}

		public Formatter(T subject, Func<string, Func<T, string>> selector)
		{
			_subject  = subject;
			_selector = selector;
		}

		public string ToString(string format, IFormatProvider formatProvider) => _selector(format)(_subject);
	}

	sealed class Formatters<T> : ISelect<T, IFormattable>, IActivateMarker<IFormatter<T>>, IActivateMarker<INamedFormatter<T>>
	{
		readonly Func<string, Func<T, string>> _select;

		public Formatters(IFormatter<T> formatter) : this(formatter.ToDelegate().Accept) {}

		public Formatters(INamedFormatter<T> formatter) : this(formatter.Get) {}

		public Formatters(Func<string, Func<T, string>> select) => _select = @select;

		public IFormattable Get(T parameter) => new Formatter<T>(parameter, _select);
	}

	sealed class DefaultApplicationDomainFormatter : IFormatter<AppDomain>
	{
		public static DefaultApplicationDomainFormatter Default { get; } = new DefaultApplicationDomainFormatter();

		DefaultApplicationDomainFormatter() {}

		public string Get(AppDomain parameter) => $"AppDomain: {parameter.FriendlyName}";
	}

	sealed class ApplicationDomainFormatter : NamedFormatter<AppDomain>
	{
		public static ApplicationDomainFormatter Default { get; } = new ApplicationDomainFormatter();

		ApplicationDomainFormatter() : base(DefaultApplicationDomainFormatter.Default,
		                                    new Formats<AppDomain>
		                                    {
			                                    {"F", x => x.FriendlyName},
			                                    {"I", x => x.Id.ToString()}
		                                    }) {}
	}

	sealed class Formats<T> : Dictionary<string, Func<T, string>> {}

	public interface INamedFormatter<in T> : ISelect<string, Func<T, string>> {}

	class NamedFormatter<T> : NamedSelection<T, string>, INamedFormatter<T>
	{
		public NamedFormatter(ISelect<T, string> @default, IEnumerable<KeyValuePair<string, Func<T, string>>> options)
			: base(@default, options) {}
	}

	sealed class DefaultFormatter : Formatter<object>
	{
		public DefaultFormatter(object subject) : base(subject, TextSelector<object>.Default) {}
	}
}
