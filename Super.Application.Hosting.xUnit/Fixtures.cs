﻿using AutoFixture;
using Super.Model.Results;

namespace Super.Application.Hosting.xUnit
{
	public class Fixtures : IResult<IFixture>
	{
		public static Fixtures Default { get; } = new Fixtures();

		Fixtures() : this(EngineParts.Default, DefaultCustomization.Default) {}

		readonly ICustomization _customization;

		readonly DefaultRelays _relays;

		public Fixtures(DefaultRelays relays, ICustomization customization)
		{
			_relays        = relays;
			_customization = customization;
		}

		public IFixture Get() => new Fixture(_relays).Customize(_customization);
	}
}