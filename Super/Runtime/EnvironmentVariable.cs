using Super.Model.Results;

namespace Super.Runtime
{
	public class EnvironmentVariable : FixedSelectedSingleton<string, string>
	{
		public EnvironmentVariable(string name) : base(EnvironmentSetting.Default, name) {}
	}
}