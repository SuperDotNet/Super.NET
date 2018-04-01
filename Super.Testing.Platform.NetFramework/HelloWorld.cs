using Super.Model.Instances;
using System;

namespace Super.Testing.Platform
{
	public sealed class HelloWorld : Instance<string>
	{
		public static HelloWorld Default { get; } = new HelloWorld();

		HelloWorld() : base($"Hello World from {AppContext.TargetFrameworkName}!") {}
	}
}