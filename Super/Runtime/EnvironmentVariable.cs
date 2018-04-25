using Super.Model.Sources;

namespace Super.Runtime
{
	public class EnvironmentVariable : FixedDeferredSingleton<string, string>
	{
		public EnvironmentVariable(string name) : base(EnvironmentSetting.Default, name) {}
	}
}