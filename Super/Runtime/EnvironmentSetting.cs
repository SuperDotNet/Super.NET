using Super.Model.Selection;

namespace Super.Runtime
{
	public sealed class EnvironmentSetting : Delegated<string, string>
	{
		public static EnvironmentSetting Default { get; } = new EnvironmentSetting();

		EnvironmentSetting() : base(System.Environment.GetEnvironmentVariable) {}
	}
}