using Super.Model.Specifications;

namespace Super.Model.Sequences.Query {
	public sealed class FirstOrDefault<T> : FirstWhere<T>
	{
		public static FirstOrDefault<T> Default { get; } = new FirstOrDefault<T>();

		FirstOrDefault() : base(Always<T>.Default) {}
	}
}