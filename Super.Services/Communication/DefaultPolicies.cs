using Polly;
using Refit;
using Super.Diagnostics;
using Super.Model.Results;

namespace Super.Application.Services.Communication
{
	public sealed class DefaultPolicies : Result<PolicyBuilder>
	{
		public static DefaultPolicies Default { get; } = new DefaultPolicies();

		DefaultPolicies() : base(ResourcePolicies.Default.In(Policy.Handle<ApiException>)) {}
	}
}