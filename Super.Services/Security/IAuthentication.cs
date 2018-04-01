using System;
using Refit;

namespace Super.Services.Security
{
	interface IAuthentication
	{
		[Get("/.auth/me")]
		IObservable<AuthenticationInformation[]> Current();
	}
}