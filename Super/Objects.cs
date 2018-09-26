using Super.Model.Collections;
using Super.Model.Selection;
using Super.Runtime;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Super
{
	// ReSharper disable once MismatchedFileName

	public static partial class ExtensionMethods
	{
		public static T With<T>(this T @this, Action<T> action)
		{
			action(@this);
			return @this;
		}


		public static TResult To<T, TResult>(this T @this, ISelect<T, TResult> select) => @this.To(select.Get);

		public static TResult To<T, TResult>(this T @this, Func<T, TResult> select) => select(@this);

		public static T If<T>(ref this bool @this, T @true, T @false) => @this ? @true : @false;

		public static TResult Return<T, TResult>(this T _, TResult result) => result;

		public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> @this,
		                                                                  IEqualityComparer<TKey> comparer = null)
			=> @this.ToDictionary(x => x.Key, x => x.Value, comparer);

		public static OrderedDictionary<TKey, TValue> ToOrderedDictionary<TKey, TValue>(
			this IEnumerable<KeyValuePair<TKey, TValue>> @this,
			IEqualityComparer<TKey> comparer = null)
			=> @this.ToOrderedDictionary(x => x.Key, x => x.Value, comparer);

		public static IReadOnlyDictionary<TKey, TValue> AsReadOnly<TKey, TValue>(
			this IEnumerable<KeyValuePair<TKey, TValue>> @this)
			=> @this as IReadOnlyDictionary<TKey, TValue> ?? new ReadOnlyDictionary<TKey, TValue>(@this.ToDictionary());

		public static TResult AsTo<TSource, TResult>(this object target, Func<TSource, TResult> transform,
		                                             Func<TResult> resolve = null)
		{
			var @default = resolve ?? (() => default);
			var result   = target is TSource source ? transform(source) : @default();
			return result;
		}

		public static (T1, T2) Pair<T1, T2>(this T1 @this, T2 other) => ValueTuple.Create(@this, other);

		public static string NullIfEmpty(this string target) => string.IsNullOrEmpty(target) ? null : target;

		public static T Self<T>(this T @this) => @this;

		public static TResult Accept<TParameter, TResult>(this TResult @this, TParameter _) => @this;

		public static IEnumerable<T> Yield<T>(this T @this)
		{
			yield return @this;
		}

		public static IEnumerable<T> Yield<T>(this T @this, T other)
		{
			yield return @this;

			yield return other;
		}

		public static IDisposable ToDisposable(this object @this) => @this as IDisposable ?? EmptyDisposable.Default;

		public static void Dispose(this IDisposable @this, object parameter) => @this.Dispose();

		public static T To<T>(this object @this)
			=> To<T>(@this, $"'{@this.GetType().FullName}' is not of type {typeof(T).FullName}.");

		public static T To<T>(this object @this, string message)
			=> @this is T result ? result : throw new InvalidOperationException(message);

		public static T Get<T>(this IServiceProvider @this)
		{
			if (@this is T instance)
			{
				return instance;
			}

			var service = @this.GetService(typeof(T));
			var result  = service != null ? service.To<T>() : default;
			return result;
		}
	}
}