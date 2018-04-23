namespace Super.Services.Security
{
	sealed class AuthenticationStateAssignment : ResponseStateAssignment
	{
		public static AuthenticationStateAssignment Default { get; } = new AuthenticationStateAssignment();

		AuthenticationStateAssignment() : base(RequestStateSelector.Default
		                                                           .Out(AppServiceAuthSession.Default)
		                                                           .Get) {}
	}
}