using AutoFixture.Kernel;
using Super.Model.Sequences;

namespace Super.Application.Hosting.xUnit
{
	sealed class DefaultTransformations : ArrayInstance<ISpecimenBuilderTransformation>
	{
		public static DefaultTransformations Default { get; } = new DefaultTransformations();

		DefaultTransformations() : base(OptionalParameterAlteration.Default, GreedyConstructorAlteration.Default) {}
	}
}