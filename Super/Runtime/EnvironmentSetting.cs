using Super.Model.Sources;

namespace Super.Runtime
{
	public sealed class EnvironmentSetting : DelegatedSource<string, string>
	{
		public static EnvironmentSetting Default { get; } = new EnvironmentSetting();

		EnvironmentSetting() : base(System.Environment.GetEnvironmentVariable) {}
	}
}