using Super.ExtensionMethods;
using Super.Model.Specifications;
using Super.Reflection;
using Super.Runtime.Activation;
using System;

namespace Super.Model.Sources.Tables
{
	public sealed class Tables<TParameter, TResult>
		: DelegatedSource<Func<TParameter, TResult>, ITable<TParameter, TResult>>
	{
		public static Tables<TParameter, TResult> Default { get; } = new Tables<TParameter, TResult>();

		Tables() : base(IsValueTypeSpecification.Default.IsSatisfiedBy(typeof(TParameter))
			                ? Activations<Func<TParameter, TResult>, ConcurrentTables<TParameter, TResult>>
			                  .Default
			                  .Reduce()
			                  .ToDelegate()
			                : new Generic<ISource<Func<TParameter, TResult>, ITable<TParameter, TResult>>>(typeof(ReferenceTables<,>))
			                  .Get(typeof(TParameter), typeof(TResult))()
			                  .ToDelegate()) {}
	}
}