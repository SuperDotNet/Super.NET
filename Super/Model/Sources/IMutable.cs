using Super.Model.Commands;

namespace Super.Model.Sources {
	public interface IMutable<T> : ISource<T>, ICommand<T> {}
}