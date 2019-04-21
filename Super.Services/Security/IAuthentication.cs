using Refit;
using System.Threading.Tasks;

namespace Super.Services.Security
{
	interface IAuthentication
	{
		[Get("/.auth/me")]
		Task<AuthenticationInformation[]> Current();
	}
}