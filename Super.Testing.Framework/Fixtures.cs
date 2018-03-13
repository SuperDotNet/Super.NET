using AutoFixture;
using Super.Model.Instances;
using Super.Runtime.Activation;

namespace Super.Testing.Framework
{
	public sealed class Fixtures<TWith> : IInstance<IFixture> where TWith : class, ICustomization
	{
		public static Fixtures<TWith> Default { get; } = new Fixtures<TWith>();
		Fixtures() : this(DefaultEngineParts.Default, Activator<TWith>.Default) {}

		readonly DefaultRelays _relays;
		readonly IInstance<TWith> _activator;

		public Fixtures(DefaultRelays relays, IInstance<TWith> activator)
		{
			_relays = relays;
			_activator = activator;
		}

		public IFixture Get() => new Fixture(_relays).Customize(_activator.Get());
	}
}