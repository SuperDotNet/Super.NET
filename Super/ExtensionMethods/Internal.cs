using Super.Model.Sources;
using Super.Model.Sources.Tables;

namespace Super.ExtensionMethods
{
	static class Internal
	{
		public static ISource<TParameter, TResult> ToReferenceStore<TParameter, TResult>(
			this ISource<TParameter, TResult> @this)
			where TResult : class
			where TParameter : class
			=> DefaultReferenceValueTables<TParameter, TResult>.Default.Get(@this.ToDelegate());
	}
}