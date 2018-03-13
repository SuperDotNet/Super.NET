using AutoFixture;

namespace Super.Testing.Framework
{
	public class OptionalParameterAlteration : EngineParts<AutoFixture.Kernel.ParameterRequestRelay>
	{
		public static OptionalParameterAlteration Default { get; } = new OptionalParameterAlteration();

		OptionalParameterAlteration() : base(relay => new CustomizationNode(new ParameterRequestRelay(relay))) {}
	}
}