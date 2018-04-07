using Super.Model.Sources;
using Super.Runtime.Activation;
using System;

namespace Super.Runtime
{
	public sealed class Exception<T> : DelegatedSource<string, T> where T : Exception
	{
		public static Exception<T> Default { get; } = new Exception<T>();

		Exception() : base(Activate<T>.New) {}
	}
}