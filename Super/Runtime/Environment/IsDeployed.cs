using System.Reflection;
using Super.Model.Sources;
using Super.Model.Specifications;

namespace Super.Runtime.Environment
{
	public sealed class IsDeployed : DelegatedResultSpecification
	{
		public static IsDeployed Default { get; } = new IsDeployed();

		IsDeployed() : this(IsAssemblyDeployed.Default, PrimaryAssembly.Default) {}

		public IsDeployed(ISpecification<Assembly> specification, ISource<Assembly> source)
			: base(specification.Out().Out(source).Singleton().Get) {}
	}
}