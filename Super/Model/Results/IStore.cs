using Super.Model.Selection.Conditions;
using Super.Runtime;

namespace Super.Model.Results
{
	public interface IStore<T> : IMutable<T>, IConditionAware<None> {}
}