using Super.Model.Commands;
using Super.Model.Selection;
using Super.Model.Selection.Stores;
using Super.Model.Specifications;
using Super.Reflection;
using Super.Reflection.Types;
using Super.Runtime;
using Super.Text;
using System;
using System.Collections.Immutable;
using System.Reactive;

namespace Super
{
	public static partial class ExtensionMethods
	{
		public static ISelect<TParameter, TResult> AsSelect<TParameter, TResult>(this ISelect<TParameter, TResult> @this)
			=> @this;

		public static ISelect<Unit, T> AsSelect<T>(this IAny<T> @this) => @this;

		public static ISelect<object, T> AsAny<T>(this IAny<T> @this) => @this;

		public static ISelect<TParameter, TResult> Default<TParameter, TResult>(this ISelect<TParameter, TResult> @this)
			=> Defaults<TParameter, TResult>.Default.Get(@this);

		public static ISelect<TParameter, TResult> If<TParameter, TResult>(this ISelect<TParameter, TResult> @this,
		                                                                   ISpecification<TParameter> @true)
			=> @this.Default().Unless(@true, @this);

		public static ISelect<TParameter, TResult> Then<TParameter, TResult>(this ISpecification<TParameter> @this,
		                                                                     ISelect<TParameter, TResult> select)
			=> select.If(@this);

		public static ISelect<TParameter, TResult> Guard<TParameter, TResult>(this ISelect<TParameter, TResult> @this)
			=> DefaultGuard<TParameter>.Default.Then(@this);

		public static ISelect<TParameter, TResult> Guard<TParameter, TResult>(this ISelect<TParameter, TResult> @this,
		                                                                      IMessage<Type> message)
			=> new AssignedGuard<TParameter>(message).Then(@this);

		public static ISelect<TParameter, TResult> Assigned<TParameter, TResult>(this ISelect<TParameter, TResult> @this)
			=> IsAssigned<TParameter>.Default.Then(@this);

		public static ISelect<TParameter, TResult> Unless<TParameter, TResult>(
			this ISelect<TParameter, TResult> @this, ISpecification<TParameter, TResult> then)
			=> @this.Unless(then, then);

		public static ISelect<TParameter, TResult> UnlessAs<TParameter, TResult, TOther>(
			this ISelect<TParameter, TResult> @this, ISelect<TOther, TResult> then)
			=> @this.Unless(IsType<TParameter, TOther>.Default, In<TParameter>.Cast(then));

		public static ISelect<TParameter, TResult> Unless<TParameter, TResult>(
			this ISelect<TParameter, TResult> @this, ISpecification<TParameter> specification, ISelect<TParameter, TResult> then)
			=> new Validated<TParameter, TResult>(specification, then, @this);

		public static ISelect<TParameter, TResult> Unless<TParameter, TResult>(
			this ISelect<TParameter, TResult> @this, ISelect<TParameter, TResult> then)
			=> @this.Unless(IsAssigned<TResult>.Default, then);

		public static ISelect<TParameter, TResult> Unless<TParameter, TResult>(
			this ISelect<TParameter, TResult> @this, ISpecification<TResult> specification, ISelect<TParameter, TResult> then)
			=> new ValidatedResult<TParameter, TResult>(specification, then, @this);

		public static ISelect<TParameter, TResult> Try<TException, TParameter, TResult>(
			this ISelect<TParameter, TResult> @this, I<TException> _) where TException : Exception
			=> new Try<TException, TParameter, TResult>(@this.ToDelegate(), @this.Default().ToDelegate());

		public static ISelect<TParameter, TResult> Configure<TParameter, TResult>(
			this ISelect<TParameter, TResult> @this, IAssignable<TParameter, TResult> assignable)
			=> new Configuration<TParameter, TResult>(@this, assignable);

		public static Func<TParameter, TResult> ToDelegate<TParameter, TResult>(this ISelect<TParameter, TResult> @this)
			=> @this.Get;

		public static Func<TParameter, TResult> ToDelegateReference<TParameter, TResult>(
			this ISelect<TParameter, TResult> @this) => Delegates<TParameter, TResult>.Default.Get(@this);

		public static ISelect<TParameter, TResult> ToStore<TParameter, TResult>(this ISelect<TParameter, TResult> @this)
			where TParameter : class => @this.ToDelegateReference().ToStore();

		public static ISelect<TParameter, TResult> ToStore<TParameter, TResult>(this Func<TParameter, TResult> @this)
			where TParameter : class => ReferenceTables<TParameter, TResult>.Default.Get(@this);

		internal static ISelect<TParameter, TResult> ToReferenceStore<TParameter, TResult>(
			this ISelect<TParameter, TResult> @this)
			where TResult : class
			where TParameter : class => new ReferenceValueTable<TParameter, TResult>(@this.Get);

		public static TResult Get<TItem, TResult>(this ISelect<ImmutableArray<TItem>, TResult> @this,
		                                          params TItem[] parameters)
			=> @this.Get(parameters.ToImmutableArray());
	}
}