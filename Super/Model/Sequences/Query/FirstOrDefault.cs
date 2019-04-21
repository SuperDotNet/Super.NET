using Super.Model.Selection.Conditions;

namespace Super.Model.Sequences.Query
{
	public sealed class FirstOrDefault<T> : FirstWhere<T>
	{
		public static FirstOrDefault<T> Default { get; } = new FirstOrDefault<T>();

		FirstOrDefault() : base(Always<T>.Default) {}
	}
}