using AutoFixture;
using AutoFixture.Kernel;
using Super.Model.Selection;
using Super.Model.Sources;
using Super.Model.Specifications;
using Super.Reflection.Types;
using System;

namespace Super.Application.Hosting.xUnit
{
	sealed class DefaultCustomization : CompositeCustomization
	{
		public static DefaultCustomization Default { get; } = new DefaultCustomization();

		DefaultCustomization() : base(SingletonCustomization.Default, new InsertCustomization(EpochSpecimen.Default),
		                              new AutoFixture.AutoMoq.AutoMoqCustomization {ConfigureMembers = true}) {}
	}

	sealed class NoSpecimenResult : Source<NoSpecimen>
	{
		public static NoSpecimenResult Default { get; } = new NoSpecimenResult();

		NoSpecimenResult() : base(new NoSpecimen()) {}
	}

	public sealed class Epoch : Source<DateTimeOffset>
	{
		public static Epoch Default { get; } = new Epoch();

		Epoch() : base(new DateTimeOffset(1976, 6, 7, 11, 18, 24, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time").BaseUtcOffset)) {}
	}

	sealed class EpochSpecimen : Specimen<DateTimeOffset>
	{
		public static EpochSpecimen Default { get; } = new EpochSpecimen();

		EpochSpecimen() : base(Epoch.Default.Get) {}
	}

	public class Specimen<T> : ISpecimenBuilder
	{
		readonly static ISpecification<object> Specification = In<object>.Cast<Type>()
		                                                                 .Out(Type<T>.Instance.Equal())
		                                                                 .Return();

		readonly ISpecification<object> _specification;
		readonly Func<T> _specimen;
		readonly NoSpecimen _none;

		public Specimen(Func<T> specimen) : this(Specification, specimen, NoSpecimenResult.Default) {}

		public Specimen(ISpecification<object> specification, Func<T> specimen, NoSpecimen none)
		{
			_specification = specification;
			_specimen = specimen;
			_none = none;
		}

		public object Create(object request, ISpecimenContext context)
			=> _specification.IsSatisfiedBy(request) ? (object)_specimen() : _none;
	}
}