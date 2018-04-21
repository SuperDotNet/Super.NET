using AutoFixture;
using Super.Model.Sources;
using Super.Runtime.Activation;

namespace Super.Application.Host.xUnit
{
	public sealed class Fixtures<TWith> : ISource<IFixture> where TWith : class, ICustomization
	{
		public static Fixtures<TWith> Default { get; } = new Fixtures<TWith>();

		Fixtures() : this(DefaultEngineParts.Default, Activator<TWith>.Default) {}

		readonly ISource<TWith> _activator;

		readonly DefaultRelays _relays;

		public Fixtures(DefaultRelays relays, ISource<TWith> activator)
		{
			_relays    = relays;
			_activator = activator;
		}

		public IFixture Get() => new Fixture(_relays).Customize(_activator.Get());
	}
}