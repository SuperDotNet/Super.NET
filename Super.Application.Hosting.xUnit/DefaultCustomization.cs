using AutoFixture;
using AutoFixture.Kernel;
using Super.Compose;
using Super.Model.Results;
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

		public ManualPropertyTypesCustomization(params Type[] types) :
			base(types.Select(x => new NoAutoPropertiesCustomization(x))) {}
	}

	sealed class NoSpecimenInstance : Instance<NoSpecimen>
	{
		public static NoSpecimenInstance Default { get; } = new NoSpecimenInstance();

		NoSpecimenInstance() : base(new NoSpecimen()) {}
	}

	public sealed class Epoch : Instance<DateTimeOffset>
	{
		public static Epoch Default { get; } = new Epoch();

		Epoch() : base(new DateTimeOffset(1976, 6, 7, 11, 18, 24,
		                                  TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time")
		                                              .BaseUtcOffset)) {}
	}

	sealed class EpochSpecimen : Specimen<DateTimeOffset>
	{
		public static EpochSpecimen Default { get; } = new EpochSpecimen();

		EpochSpecimen() : base(Epoch.Default.Get) {}
	}

	public class Specimen<T> : ISpecimenBuilder
	{
		readonly static Func<object, bool> Condition = A.This(IsOf<Type>.Default)
		                                                .Then()
		                                                .And(Start.A.Selection.Of.Any.AndOf<Type>()
		                                                          .By.Cast.Or.Throw.Then()
		                                                          .Select(Type<T>.Instance.Equal())
		                                                          .Selector());

		readonly Func<object, bool> _condition;
		readonly Func<T>            _specimen;
		readonly NoSpecimen         _none;

		public Specimen(Func<T> specimen) : this(Condition, specimen, NoSpecimenInstance.Default) {}

		public Specimen(Func<object, bool> condition, Func<T> specimen, NoSpecimen none)
		{
			_condition = condition;
			_specimen  = specimen;
			_none      = none;
		}

		public object Create(object request, ISpecimenContext context)
			=> _condition(request) ? (object)_specimen() : _none;
	}
}