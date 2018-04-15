namespace Super.Application.Testing
{
	public sealed class AutoMoqCustomization : AutoFixture.AutoMoq.AutoMoqCustomization
	{
		public static AutoMoqCustomization Default { get; } = new AutoMoqCustomization();

		AutoMoqCustomization() => ConfigureMembers = true;
	}
}