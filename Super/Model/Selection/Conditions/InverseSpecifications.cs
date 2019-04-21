using Super.Model.Selection.Stores;

namespace Super.Model.Selection.Conditions
{
	sealed class InverseSpecifications<T> : ReferenceValueStore<ICondition<T>, InverseCondition<T>>
	{
		public static InverseSpecifications<T> Default { get; } = new InverseSpecifications<T>();

		InverseSpecifications() : base(x => new InverseCondition<T>(x)) {}
	}
}