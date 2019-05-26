using AutoFixture.Kernel;
using Super.Model.Results;

namespace Super.Application.Hosting.xUnit
{
	sealed class NoSpecimenInstance : Instance<NoSpecimen>
	{
		public static NoSpecimenInstance Default { get; } = new NoSpecimenInstance();

		NoSpecimenInstance() : base(new NoSpecimen()) {}
	}
}