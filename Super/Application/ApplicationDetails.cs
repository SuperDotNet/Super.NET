using Super.Reflection.Assemblies;
using System;

namespace Super.Application
{
	public sealed class ApplicationDetails
	{
		public ApplicationDetails(AssemblyDetails assembly, Uri companyUri, DateTimeOffset? deployment)
		{
			Assembly = assembly;
			CompanyUri = companyUri;
			Deployment = deployment;
		}

		public AssemblyDetails Assembly { get; }

		public Uri CompanyUri { get; set; }

		public DateTimeOffset? Deployment { get; }
	}
}
