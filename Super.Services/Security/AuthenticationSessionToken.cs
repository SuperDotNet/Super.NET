using Super.Runtime;

namespace Super.Services.Security
{
	sealed class AuthenticationSessionToken : EnvironmentVariable
	{
		public static AuthenticationSessionToken Default { get; } = new AuthenticationSessionToken();

		AuthenticationSessionToken() : base(nameof(AuthenticationSessionToken)) {}
	}
}