using Super.ExtensionMethods;

namespace Super.Services.Security
{
	sealed class AppServiceAuthSession : ResponseState
	{
		const string Name = nameof(AppServiceAuthSession);

		public static AppServiceAuthSession Default { get; } = new AppServiceAuthSession();

		AppServiceAuthSession() : base(Name,
		                               new RequestStateValue(Name).Or(AuthenticationSessionToken.Default.Allow())
		                                                          .ToDelegate()) {}
	}
}