using Super.Model.Sources;

namespace Super.Application.Hosting.BenchmarkDotNet
{
	class EnvironmentVariable : Source<global::BenchmarkDotNet.Jobs.EnvironmentVariable>
	{
		public EnvironmentVariable(string name, string value)
			: this(new global::BenchmarkDotNet.Jobs.EnvironmentVariable(name, value)) {}

		public EnvironmentVariable(global::BenchmarkDotNet.Jobs.EnvironmentVariable instance) : base(instance) {}
	}

	class EnvironmentVariable<T> : EnvironmentVariable
	{
		public EnvironmentVariable(string name, T value) : this(name, value.ToString()) {}

		public EnvironmentVariable(string name, string value) : base(name, value) {}
	}
}