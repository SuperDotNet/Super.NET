using Super.Model.Sources;
using Super.Reflection;

namespace Super.Aspects
{
	static class Source
	{
		public static I<ISource<T1, T2>> Infer<T1, T2>(ISource<T1, T2> _) => I<ISource<T1, T2>>.Default;
	}
}