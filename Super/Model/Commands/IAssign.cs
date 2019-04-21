using Super.Runtime;

namespace Super.Model.Commands
{
	public interface IAssign<TKey, TValue> : ICommand<Pair<TKey, TValue>> {}
}