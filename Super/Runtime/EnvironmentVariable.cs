using Super.Model.Instances;

namespace Super.Runtime
{
	public class EnvironmentVariable : Instance<string, string>
	{
		public EnvironmentVariable(string name) : base(EnvironmentSetting.Default, name) {}
	}
}