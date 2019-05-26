using System;
using Super.Text;
using Super.Text.Formatting;

namespace Super.Testing.Objects
{
	sealed class ApplicationDomainFormatter : Selection<AppDomain, string>, ISelectFormatter<AppDomain>
	{
		public static ApplicationDomainFormatter Default { get; } = new ApplicationDomainFormatter();

		ApplicationDomainFormatter()
			: base(DefaultApplicationDomainFormatter.Default,
			       ApplicationDomainName.Default,
			       ApplicationDomainIdentifier.Default) {}
	}
}