using JetBrains.Annotations;
using Super.Model.Selection.Conditions;
using Super.Runtime;
using Super.Runtime.Activation;

namespace Super.Model.Results
{
	public class Store<T> : Mutable<T>, IStore<T>, IActivateUsing<IMutable<T>>
	{
		[UsedImplicitly]
		public Store(IMutable<T> mutable) : this(mutable.Select(IsAssigned<T>.Default).ToSelect().ToCondition(),
		                                         mutable) {}

		public Store(ICondition<None> condition, IMutable<T> mutable) : base(mutable) => Condition = condition;

		public ICondition<None> Condition { get; }
	}
}