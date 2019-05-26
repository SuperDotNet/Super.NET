using Super.Model.Selection.Conditions;

namespace Super.Runtime
{
	sealed class IsModified<T> : InverseCondition<T>
	{
		public static IsModified<T> Default { get; } = new IsModified<T>();

		IsModified() : base(IsDefault<T>.Default) {}
	}
}