namespace Super.Model.Sequences.Query {
	public sealed class Assigned<T> : Where<T> where T : class
	{
		public static Assigned<T> Default { get; } = new Assigned<T>();

		Assigned() : base(x => x != null) {}
	}
}