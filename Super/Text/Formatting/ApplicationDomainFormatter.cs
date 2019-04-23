using System;

namespace Super.Text.Formatting
{
	// TODO: Move to Testing Objects.
	sealed class ApplicationDomainFormatter : TextSelect<AppDomain, string>, ISelectFormatter<AppDomain>
	{
		public static ApplicationDomainFormatter Default { get; } = new ApplicationDomainFormatter();

		ApplicationDomainFormatter()
			: base(DefaultApplicationDomainFormatter.Default,
			       ApplicationDomainName.Default, ApplicationDomainIdentifier.Default) {}
	}
}