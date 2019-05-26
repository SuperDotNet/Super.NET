using System.Threading.Tasks;
using Refit;

namespace Super.Services.Security
{
	interface IAuthentication
	{
		[Get("/.auth/me")]
		Task<AuthenticationInformation[]> Current();
	}
}