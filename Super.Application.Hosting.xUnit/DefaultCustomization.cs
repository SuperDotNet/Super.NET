﻿using AutoFixture;
using AutoFixture.Kernel;
using Super.Model.Sources;
using Super.Model.Specifications;
using Super.Reflection;
using Super.Reflection.Types;
using System;
using System.Linq;
using System.Threading;

namespace Super.Application.Hosting.xUnit
{
	sealed class DefaultCustomization : CompositeCustomization
	{
		public static DefaultCustomization Default { get; } = new DefaultCustomization();

		DefaultCustomization() : base(ManualPropertyTypesCustomization.Default,
		                              SingletonCustomization.Default,
		                              new InsertCustomization(EpochSpecimen.Default),
		                              new AutoFixture.AutoMoq.AutoMoqCustomization {ConfigureMembers = true}) {}
	}

	sealed class ManualPropertyTypesCustomization : CompositeCustomization
	{
		public static ManualPropertyTypesCustomization Default { get; } = new ManualPropertyTypesCustomization();

		ManualPropertyTypesCustomization() : this(typeof(Thread)) {}

		public ManualPropertyTypesCustomization(params Type[] types) : base(types.Select(x => new NoAutoPropertiesCustomization(x))) {}
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
		readonly static ISpecification<object> Specification = Type<T>.Instance.Equal()
		                                                              .Out(x => x.Out(I<object>.Default))
		                                                              .Out();

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