using Polly;
using Refit;
using Super.Diagnostics;
using Super.Model.Sources;

namespace Super.Services
{
	public sealed class DefaultPolicies : Source<PolicyBuilder>
	{
		public static DefaultPolicies Default { get; } = new DefaultPolicies();

		DefaultPolicies() : base(ResourcePolicies.Default.Get(Policy.Handle<ApiException>())) {}
	}
}