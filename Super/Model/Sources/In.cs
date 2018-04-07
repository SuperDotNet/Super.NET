using Super.ExtensionMethods;
using Super.Model.Containers;
using Super.Reflection;
using System;

namespace Super.Model.Sources
{
	public static class In<T>
	{
		public static ISource<T, TResult> New<TResult>(TResult @this) => @this.ToSource(I<T>.Default);

		public static ISource<T, TResult> Select<TResult>(ISelect<TResult> select) => Select(select.Out<T>());

		public static ISource<T, TResult> Select<TResult>(Func<T, TResult> @this)
			=> Sources<T, TResult>.Default.Get(@this);
	}

	public static class Source
	{
		public static ISource<TParameter, TResult> From<TParameter, TResult>(Func<TParameter, TResult> source)
			=> In<TParameter>.Select(source);
	}
}