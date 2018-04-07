using Super.ExtensionMethods;
using Super.Model.Collections;
using Super.Model.Sources;
using Super.Model.Sources.Alterations;
using Super.Model.Sources.Tables;
using Super.Reflection;
using Super.Runtime.Activation;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;

namespace Super.Runtime.Invocation
{
	class Deferred<TParameter, TResult> : DecoratedSource<TParameter, TResult>,
	                                               IActivateMarker<ISource<TParameter, TResult>>
	{
		public Deferred(ISource<TParameter, TResult> source)
			: this(source, Tables<TParameter, TResult>.Default.Get(_ => default)) {}

		public Deferred(ISource<TParameter, TResult> source, IMutable<TParameter, TResult> mutable)
			: base(mutable.Or(new ConfiguringSource<TParameter, TResult>(source, mutable))) {}
	}

	sealed class Striped<TParameter, TResult> : DecoratedSource<TParameter, TResult>,
	                                            IActivateMarker<ISource<TParameter, TResult>>
	{
		public Striped(ISource<TParameter, TResult> source)
			: base(source.Or(source.New(I<Deferred<TParameter, TResult>>.Default)
			                       .ToDelegate()
			                       .To(I<Stripe<TParameter, TResult>>.Default))) {}
	}

	sealed class StripedAlteration<TParameter, TResult> : DelegatedAlteration<ISource<TParameter, TResult>>
	{
		public static StripedAlteration<TParameter, TResult> Default { get; } = new StripedAlteration<TParameter, TResult>();

		StripedAlteration() : base(I<Striped<TParameter, TResult>>.Default.From) {}
	}

	sealed class ProtectAlteration<TParameter, TResult> : DelegatedAlteration<ISource<TParameter, TResult>>
	{
		public static ProtectAlteration<TParameter, TResult> Default { get; } = new ProtectAlteration<TParameter, TResult>();

		ProtectAlteration() : base(I<Protect<TParameter, TResult>>.Default.From) {}
	}

	sealed class Protect<TParameter, TResult> : ISource<TParameter, TResult>, IActivateMarker<ISource<TParameter, TResult>>
	{
		readonly Func<TParameter, TResult> _source;

		public Protect(ISource<TParameter, TResult> source) : this(source.ToDelegate()) {}

		public Protect(Func<TParameter, TResult> source) => _source = source;

		public TResult Get(TParameter parameter)
		{
			lock (_source)
			{
				return _source(parameter);
			}
		}
	}

	sealed class Stripe<TParameter, TResult> : ISource<TParameter, TResult>, IActivateMarker<Func<TParameter, TResult>>
	{
		readonly static Func<TParameter, object> Lock = Locks<TParameter>.Default.ToDelegate();

		readonly Func<TParameter, object>  _lock;
		readonly Func<TParameter, TResult> _source;

		public Stripe(Func<TParameter, TResult> source) : this(Lock, source) {}

		public Stripe(Func<TParameter, object> @lock, Func<TParameter, TResult> source)
		{
			_lock   = @lock;
			_source = source;
		}

		public TResult Get(TParameter parameter)
		{
			lock (_lock(parameter))
			{
				return _source(parameter);
			}
		}
	}

	/// <summary>
	/// Attribution: https://github.com/i3arnon/AsyncUtilities
	/// </summary>
	// ReSharper disable once PossibleInfiniteInheritance
	sealed class LockItem<T> : ISource<int, (ImmutableArray<T> Items, int Mask)>
	{
		public static LockItem<T> Default { get; } = new LockItem<T>();

		LockItem() : this(Activation<T>.Default.ToDelegate()) {}

		readonly Func<T> _create;

		public LockItem(Func<T> create) => _create = create;

		public (ImmutableArray<T> Items, int Mask) Get(int parameter)
		{
			var mask   = GetStripeMask(parameter);
			var result = (new Repeat<T>(mask + 1, _create).ToImmutableArray(), mask);
			return result;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		static int GetStripeMask(int stripes)
		{
			stripes |= stripes >> 1;
			stripes |= stripes >> 2;
			stripes |= stripes >> 4;
			stripes |= stripes >> 8;
			stripes |= stripes >> 16;
			return stripes;
		}
	}

	sealed class Locks<T> : Locks<T, object>
	{
		public static Locks<T> Default { get; } = new Locks<T>();

		Locks() : base(System.Environment.ProcessorCount) {}
	}

	/// <summary>
	/// Attribution: https://github.com/i3arnon/AsyncUtilities
	/// </summary>
	/// <typeparam name="TKey"></typeparam>
	/// <typeparam name="TLock"></typeparam>
	public class Locks<TKey, TLock> : ISource<TKey, TLock>
	{
		readonly int                     _mask;
		readonly ImmutableArray<TLock>   _stripes;
		readonly IEqualityComparer<TKey> _comparer;

		public Locks(int stripes) : this(LockItem<TLock>.Default.Get(stripes), EqualityComparer<TKey>.Default) {}

		public Locks((ImmutableArray<TLock> Items, int Mask) item, IEqualityComparer<TKey> comparer)
			: this(item.Mask, item.Items, comparer) {}

		public Locks(int mask, ImmutableArray<TLock> stripes, IEqualityComparer<TKey> comparer)
		{
			_mask     = mask;
			_stripes  = stripes;
			_comparer = comparer;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		int GetStripe(TKey key) => SmearHashCode(_comparer.GetHashCode(key) & int.MaxValue) & _mask;

		// ReSharper disable once ComplexConditionExpression
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		static int SmearHashCode(int hashCode)
		{
			hashCode ^= hashCode >> 20 ^ hashCode >> 12;
			return hashCode ^ hashCode >> 7 ^ hashCode >> 4;
		}

		public TLock Get(TKey parameter) => _stripes[GetStripe(parameter)];
	}
}