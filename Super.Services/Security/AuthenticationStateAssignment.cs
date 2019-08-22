using Super.Application.Services.Communication;

namespace Super.Application.Services.Security
{
	sealed class AuthenticationStateAssignment : ResponseStateAssignment
	{
		public static AuthenticationStateAssignment Default { get; } = new AuthenticationStateAssignment();

		AuthenticationStateAssignment() : base(RequestStateSelector.Default
		                                                           .Select(AppServiceAuthSession.Default)
		                                                           .Get) {}
	}
}