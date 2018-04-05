using Super.Model.Commands;

namespace Super.Model.Instances {
	public interface IMutable<T> : IInstance<T>, ICommand<T> {}
}