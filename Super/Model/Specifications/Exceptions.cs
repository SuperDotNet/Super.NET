using System;
using Super.Model.Sources;

namespace Super.Model.Specifications
{
	public static class Exceptions
	{
		public static Exceptions<T> From<T>(Func<string, T> create) where T : Exception => new Exceptions<T>(create);
	}

	public sealed class Exceptions<T> : DelegatedSource<string, T> where T : Exception
	{
		public Exceptions(Func<string, T> source) : base(source) {}
	}
}