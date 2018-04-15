using Serilog;
using Super.Diagnostics.Logging;
using Super.Text;
using Super.Text.Formatting;
using System;

namespace Super.Testing.Objects
{
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


	sealed class ApplicationDomainFormatter : TextSelect<AppDomain, string>, ISelectFormatter<AppDomain>
	{
		public static ApplicationDomainFormatter Default { get; } = new ApplicationDomainFormatter();

		ApplicationDomainFormatter()
			: base(DefaultApplicationDomainFormatter.Default,
			       ApplicationDomainName.Default,
			       ApplicationDomainIdentifier.Default) {}
	}

	sealed class DefaultApplicationDomainFormatter : IFormatter<AppDomain>
	{
		public static DefaultApplicationDomainFormatter Default { get; } = new DefaultApplicationDomainFormatter();

		DefaultApplicationDomainFormatter() {}

		public string Get(AppDomain parameter) => $"AppDomain: {parameter.FriendlyName}";
	}

	sealed class Template : LogMessage<AppDomain>
	{
		public Template(ILogger logger) : base(logger, "Hello World: {@AppDomain:F}") {}
	}
}
