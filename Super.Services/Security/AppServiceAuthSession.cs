using Super.Reflection;

namespace Super.Services.Security
{
	sealed class AppServiceAuthSession : ResponseState
	{
		const string Name = nameof(AppServiceAuthSession);

		public static AppServiceAuthSession Default { get; } = new AppServiceAuthSession();

		AppServiceAuthSession() : base(Name, AuthenticationSessionToken.Default
		                                                               .Out(I<object>.Default)
		                                                               .Unless(new RequestStateValue(Name))
		                                                               .Get) {}
	}
}