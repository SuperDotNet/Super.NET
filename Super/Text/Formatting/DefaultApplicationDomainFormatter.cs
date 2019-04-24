using Super.Model.Selection;
using System;

namespace Super.Text.Formatting
{
	sealed class DefaultApplicationDomainFormatter : Select<AppDomain, string>, IFormatter<AppDomain>
	{
		public static DefaultApplicationDomainFormatter Default { get; } = new DefaultApplicationDomainFormatter();

		DefaultApplicationDomainFormatter() : base(x => $"AppDomain: {x.FriendlyName}") {}
	}
}