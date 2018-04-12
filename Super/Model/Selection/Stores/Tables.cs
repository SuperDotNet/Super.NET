using Super.ExtensionMethods;
using Super.Model.Specifications;
using Super.Reflection;
using Super.Runtime.Activation;
using System;
using System.Collections.Concurrent;

namespace Super.Model.Selection.Stores
{
	public sealed class Tables<TParameter, TResult>
		: Delegated<Func<TParameter, TResult>, ITable<TParameter, TResult>>
	{
		public static Tables<TParameter, TResult> Default { get; } = new Tables<TParameter, TResult>();

		Tables() : base(IsValueTypeSpecification.Default.IsSatisfiedBy(typeof(TParameter))
			                ? Activations<Func<TParameter, TResult>, ConcurrentTables<TParameter, TResult>>
			                  .Default
			                  .Out(Activation<ConcurrentDictionary<TParameter, TResult>>.Default.Get)
			                  .ToDelegate()
			                : new Generic<ISelect<Func<TParameter, TResult>, ITable<TParameter, TResult>>>(typeof(ReferenceTables<,>))
			                  .Get(typeof(TParameter), typeof(TResult))()
			                  .ToDelegate()) {}
	}

	public static class Tables
	{
		public static ITable<TParameter, TResult> From<TParameter, TResult>(this Func<TParameter, TResult> @this)
			=> Tables<TParameter, TResult>.Default.Get(@this);
	}
}