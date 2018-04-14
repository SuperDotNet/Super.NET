namespace Super.Testing.Framework
{
	public sealed class AutoMoqCustomization : AutoFixture.AutoMoq.AutoMoqCustomization
	{
		public static AutoMoqCustomization Default { get; } = new AutoMoqCustomization();

		AutoMoqCustomization() => ConfigureMembers = true;
	}
}