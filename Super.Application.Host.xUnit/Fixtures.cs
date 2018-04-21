using AutoFixture;
using Super.Model.Sources;

namespace Super.Application.Host.xUnit
{
	/*public sealed class Fixtures<TWith> : Fixtures where TWith : class, ICustomization
	{
		public Fixtures(DefaultRelays relays) : this(relays, Activator<TWith>.Default) {}

		public Fixtures(DefaultRelays relays, ISource<TWith> activator) : base(relays, activator.Get()) {}
	}*/

	public class Fixtures : ISource<IFixture>
	{
		public static Fixtures Default { get; } = new Fixtures();

		Fixtures() : this(EngineParts.Default, DefaultCustomization.Default) {}

		readonly DefaultRelays _relays;
		readonly ICustomization _customization;

		public Fixtures(DefaultRelays relays, ICustomization customization)
		{
			_relays    = relays;
			_customization = customization;
		}

		public IFixture Get() => new Fixture(_relays).Customize(_customization);
	}
}