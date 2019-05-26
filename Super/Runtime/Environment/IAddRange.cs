using Super.Model.Commands;
using Super.Model.Sequences;

namespace Super.Runtime.Environment
{
	public interface IAddRange<T> : ICommand<Store<T>> {}
}