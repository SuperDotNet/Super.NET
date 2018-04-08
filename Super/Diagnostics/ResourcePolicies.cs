using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Polly;
using Super.Model.Selection.Alterations;

namespace Super.Diagnostics
{
	public sealed class ResourcePolicies : IAlteration<PolicyBuilder>
	{
		public static ResourcePolicies Default { get; } = new ResourcePolicies();

		ResourcePolicies() {}

		public PolicyBuilder Get(PolicyBuilder parameter) => parameter.Or<IOException>()
		                                                              .Or<HttpRequestException>()
		                                                              .Or<TaskCanceledException>();
	}
}