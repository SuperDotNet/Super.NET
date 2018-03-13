using Super.Model.Sources;

namespace Super.Model.Commands
{
	sealed class Adapters<T> : ReferenceStore<ICommand<T>, CommandSourceAdapter<T>>
	{
		public static Adapters<T> Default { get; } = new Adapters<T>();

		Adapters() : base(x => new CommandSourceAdapter<T>(x)) {}
	}
}