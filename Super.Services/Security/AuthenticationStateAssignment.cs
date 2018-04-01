using Super.ExtensionMethods;

namespace Super.Services.Security
{
	sealed class AuthenticationStateAssignment : ResponseStateAssignment
	{
		public static AuthenticationStateAssignment Default { get; } = new AuthenticationStateAssignment();

		AuthenticationStateAssignment() : base(RequestStateCoercer.Default.Out(AppServiceAuthSession.Default).ToDelegate()) {}
	}
}