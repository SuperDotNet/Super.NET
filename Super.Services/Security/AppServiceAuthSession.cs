using Super.Compose;

namespace Super.Application.Services.Security
{
	sealed class AppServiceAuthSession : ResponseState
	{
		const string Name = nameof(AppServiceAuthSession);

		public static AppServiceAuthSession Default { get; } = new AppServiceAuthSession();

		AppServiceAuthSession() : base(Name, Start.A.Selection(new RequestStateValue(Name))
		                                          .Unless(AuthenticationSessionToken.Default.ToDelegate())
		                                          .Get) {}
	}
}