using System;

namespace Super.Text.Formatting
{
	// TODO: Move to Testing Objects.
	sealed class ApplicationDomainName : Format<AppDomain>
	{
		public static ApplicationDomainName Default { get; } = new ApplicationDomainName();

		ApplicationDomainName() : base("F", x => x.FriendlyName) {}
	}
}