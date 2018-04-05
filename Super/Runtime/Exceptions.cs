using Super.Model.Sources;
using System;

namespace Super.Runtime
{
	public static class Exceptions
	{
		
	}

	public sealed class Exceptions<T> : DelegatedSource<string, T> where T : Exception
	{
		public Exceptions(Func<string, T> source) : base(source) {}
	}
}