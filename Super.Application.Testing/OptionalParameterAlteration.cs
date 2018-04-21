using AutoFixture.Kernel;

namespace Super.Application.Testing
{
	sealed class OptionalParameterAlteration : BuilderSelection<AutoFixture.Kernel.ParameterRequestRelay>
	{
		public static OptionalParameterAlteration Default { get; } = new OptionalParameterAlteration();

		OptionalParameterAlteration() : base(relay => new ParameterRequestRelay(relay)) {}
	}

	sealed class GreedyConstructorAlteration : BuilderSelection<MethodInvoker>
	{
		public static GreedyConstructorAlteration Default { get; } = new GreedyConstructorAlteration();

		GreedyConstructorAlteration()
			: base(new MethodInvoker(new CompositeMethodQuery(new GreedyConstructorQuery(), new FactoryMethodQuery())).Accept) {}
	}
}