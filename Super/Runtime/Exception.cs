using Super.Runtime.Activation;
using System;
using Super.Model.Selection;

namespace Super.Runtime
{
	public sealed class Exception<T> : Delegated<string, T> where T : Exception
	{
		public static Exception<T> Default { get; } = new Exception<T>();

		Exception() : base(Activate<T>.New) {}
	}
}