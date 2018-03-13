using System;
using Super.ExtensionMethods;
using Super.Model.Sources.Tables;
using Super.Reflection;

namespace Super.Model.Sources
{
	sealed class Stores<TParameter, TResult> : DelegatedSource<Func<TParameter, TResult>, ISource<TParameter, TResult>>
	{
		public static Stores<TParameter, TResult> Default { get; } = new Stores<TParameter, TResult>();

		Stores() :
			base(IsValueTypeSpecification.Default.IsSatisfiedBy(Types<TParameter>.Key)
				     ? Sources<TParameter, TResult>.Default.ToDelegate()
				     : new Generic<ISource<Func<TParameter, TResult>, ISource<TParameter, TResult>>>(typeof(ReferenceTables<,>))
				       .Get(typeof(TParameter), typeof(TResult))()
				       .Get) {}
	}
}