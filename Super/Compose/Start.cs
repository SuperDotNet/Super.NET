using Super.Compose.Extents;
using Super.Model.Commands;
using Super.Model.Results;
using Super.Model.Selection;
using Super.Model.Selection.Conditions;
using Super.Runtime;
using System;
using Action = Super.Model.Selection.Adapters.Action;
using Context = Super.Compose.Results.Context;

namespace Super.Compose
{
	public static class A
	{
		public static T Of<T>() => Start.An.Instance<T>();

		public static T This<T>(T instance) => instance;

		public static Type Type<T>() => Reflection.Types.Type<T>.Instance;

		public static Type Metadata<T>() => Reflection.Types.Type<T>.Metadata;

		public static ISelect<T, T> Self<T>() => Model.Selection.Self<T>.Default;
	}

	public static class Make
	{
		public static T A<T>() => Start.An.Instance<T>();
	}

	public static class Start
	{
		public static ModelContext A { get; } = ModelContext.Default;

		public static VowelContext An { get; } = VowelContext.Default;
	}

	public sealed class VowelContext
	{
		public static VowelContext Default { get; } = new VowelContext();

		VowelContext() : this(Extents.Extents.Default) {}

		public VowelContext(Extents.Extents extent) => Extent = extent;

		public Extents.Extents Extent { get; }
	}

	public static class Extensions
	{
		public static Extent<T> Extent<T>(this VowelContext _) => Extents.Extent<T>.Default;

		public static Activation<T> Activation<T>(this VowelContext _) => Extents.Activation<T>.Instance;

		public static T Instance<T>(this VowelContext _) => Extents.Activation<T>.Instance.Activate();

		public static T Instance<T>(this VowelContext _, T instance) => instance;

		/* Extents  */

		/* Results */
		public static Results.Extent<T> Of<T>(this Context @this) => @this.Of.Type<T>();

		public static Results.Extent<T> Result<T>(this ModelContext @this) => @this.Result.Of.Type<T>();

		public static IResult<T> Result<T>(this ModelContext _, IResult<T> result) => result;

		public static IResult<T> Result<T>(this ModelContext @this, T instance) => @this.Result<T>().By.Using(instance);

		public static IResult<T> Result<T>(this ModelContext @this, Func<T> result)
			=> @this.Result<T>().By.Calling(result);

		/* Condition */
		public static Conditions.Extent<T> Of<T>(this Conditions.Context @this) => @this.Of.Type<T>();

		public static Conditions.Extent<T> Condition<T>(this ModelContext @this) => @this.Condition.Of.Type<T>();

		public static ICondition<T> Condition<T>(this ModelContext _, ICondition<T> result) => result;

		/* Command */
		public static Commands.Extent<T> Of<T>(this Commands.Context @this) => @this.Of.Type<T>();

		public static Commands.Extent<T> Command<T>(this ModelContext @this) => @this.Command.Of.Type<T>();

		public static Model.Selection.Adapters.Action<T> Command<T>(this ModelContext _, ICommand<T> result)
			=> result.ToDelegateReference();

		public static Action Calling(this Commands.Extent<None> _, System.Action body) => new Action(body);

		/* Selection */
		public static Selections.Extent<T> Of<T>(this Selections.Context @this) => @this.Of.Type<T>();

		public static Selections.Extent<T> Selection<T>(this ModelContext @this) => @this.Selection.Of.Type<T>();

		/*public static ISelect<None, T> Selection<T>(this ModelContext @this, IResult<T> result)
			=> @this.Selection(result.Out());*/

		public static ISelect<TIn, TOut> Selection<TIn, TOut>(this ModelContext _, Func<TIn, TOut> select)
			=> new Select<TIn, TOut>(select);

		public static ISelect<TIn, TOut> Selection<TIn, TOut>(this ModelContext _, ISelect<TIn, TOut> select) => select;

		/* Generics */
	}

	public sealed class ModelContext
	{
		public static ModelContext Default { get; } = new ModelContext();

		ModelContext() : this(() => Conditions.Context.Default, () => Context.Default,
		                      () => Commands.Context.Default, () => Selections.Context.Default) {}

		readonly Func<Conditions.Context> _condition;
		readonly Func<Context>            _result;
		readonly Func<Commands.Context>   _command;
		readonly Func<Selections.Context> _selection;

		// ReSharper disable once TooManyDependencies
		public ModelContext(Func<Conditions.Context> condition, Func<Context> result, Func<Commands.Context> command,
		                    Func<Selections.Context> selection)
		{
			_condition = condition;
			_result    = result;
			_command   = command;
			_selection = selection;
		}

		public Conditions.Context Condition => _condition();

		public Context Result => _result();

		public Commands.Context Command => _command();

		public Selections.Context Selection => _selection();

		public Generics.Context Generic(Type definition) => new Generics.Context(definition);
	}
}