using System;

namespace Super.Testing.Platform
{
	public sealed class HelloWorld : Text.Text
	{
		public static HelloWorld Default { get; } = new HelloWorld();

		HelloWorld() : base($"Hello World from {AppContext.TargetFrameworkName}!") {}
	}
}