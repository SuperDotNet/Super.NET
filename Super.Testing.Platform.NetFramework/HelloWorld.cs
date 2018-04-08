using System;
using Super.Model.Sources;

namespace Super.Testing.Platform
{
	public sealed class HelloWorld : Source<string>
	{
		public static HelloWorld Default { get; } = new HelloWorld();

		HelloWorld() : base($"Hello World from {AppContext.TargetFrameworkName}!") {}
	}
}