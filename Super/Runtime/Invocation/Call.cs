using Super.Model.Selection;
using System;

namespace Super.Runtime.Invocation
{
	public interface IInvoke<T> : ISelect<Func<T>, T> {}

	class Invoke<T> : Select<Func<T>, T>, IInvoke<T>
	{
		public Invoke(Func<Func<T>, T> source) : base(source) {}
	}

	sealed class Call<T> : Invoke<T>
	{
		public static Call<T> Default { get; } = new Call<T>();

		Call() : base(func => func()) {}
	}
}