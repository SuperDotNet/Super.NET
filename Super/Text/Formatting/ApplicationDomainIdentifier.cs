using System;

namespace Super.Text.Formatting
{
	// TODO: Move to Testing Objects.
	sealed class ApplicationDomainIdentifier : Format<AppDomain>
	{
		public static ApplicationDomainIdentifier Default { get; } = new ApplicationDomainIdentifier();

		ApplicationDomainIdentifier() : base("I", x => x.Id.ToString()) {}
	}
}