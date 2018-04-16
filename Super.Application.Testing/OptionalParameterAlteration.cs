using AutoFixture;

namespace Super.Application.Testing
{
	public class OptionalParameterAlteration : BuilderSelection<AutoFixture.Kernel.ParameterRequestRelay>
	{
		public static OptionalParameterAlteration Default { get; } = new OptionalParameterAlteration();

		OptionalParameterAlteration() : base(relay => new CustomizationNode(new ParameterRequestRelay(relay))) {}
	}
}