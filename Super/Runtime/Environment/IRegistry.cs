using Super.Model.Commands;
using Super.Model.Sequences;

namespace Super.Runtime.Environment
{
	public interface IRegistry<T> : IArray<T>, IAddRange<T>, ICommand<T> {}
}