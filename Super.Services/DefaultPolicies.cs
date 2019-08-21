using Polly;
using Refit;
using Super.Diagnostics;
using Super.Model.Results;

namespace Super.Application.Services
{
	public sealed class DefaultPolicies : Instance<PolicyBuilder>
	{
		public static DefaultPolicies Default { get; } = new DefaultPolicies();

		DefaultPolicies() : base(ResourcePolicies.Default.Get(Policy.Handle<ApiException>())) {}
	}
}