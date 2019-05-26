﻿namespace Super.Model.Sequences.Query
{
	/*public sealed class FirstOrDefault<T> : FirstWhere<T>
	{
		public static FirstOrDefault<T> Default { get; } = new FirstOrDefault<T>();

		FirstOrDefault() : base(Always<T>.Default) {}
	}*/

	sealed class FirstOrDefault<T> : LimitAware, IReduce<T>
	{
		public static FirstOrDefault<T> Default { get; } = new FirstOrDefault<T>();

		FirstOrDefault() : base(1) {}

		public T Get(Store<T> parameter) => parameter.Length > 0 ? parameter.Instance[0] : default;
	}
}