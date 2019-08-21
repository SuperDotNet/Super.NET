using Refit;
using System.Threading.Tasks;

namespace Super.Application.Services.Security
{
	interface IAuthentication
	{
		[Get("/.auth/me")]
		Task<AuthenticationInformation[]> Current();
	}
}