using Super.Compose;
using Super.Model.Commands;
using Super.Model.Results;
using Super.Model.Selection.Alterations;
using Super.Model.Selection.Conditions;
using Super.Reflection.Types;
using Super.Runtime;
using Super.Runtime.Activation;
using Super.Runtime.Invocation;
using Super.Runtime.Objects;
using System;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Super.Model.Selection.Adapters
{
	public sealed class Selection<TIn, TOut> : Select<TIn, TOut>
	{
		public static implicit operator Selection<TIn, TOut>(TOut instance)
			=> new Selection<TIn, TOut>(new FixedResult<TIn, TOut>(instance));

		public static implicit operator Selection<TIn, TOut>(Func<TOut> instance)
			=> new Selection<TIn, TOut>(new DelegatedResult<TIn, TOut>(instance));

		public static implicit operator Selection<TIn, TOut>(Func<TIn, TOut> instance)
			=> new Selection<TIn, TOut>(instance);

		public Selection(ISelect<TIn, TOut> @select) : base(@select.Get) {}

		public Selection(Func<TIn, TOut> @select) : base(@select) {}
	}

	public class ResultDelegateSelector<_, T> : Selector<_, Func<T>>
	{
		public ResultDelegateSelector(ISelect<_, Func<T>> subject) : base(subject) {}

		public Selector<_, Func<T>> Singleton() => Select(SingletonDelegate<T>.Default);

		public Selector<_, T> Invoke() => Select(Call<T>.Default);
	}

	public class ResultSelector<T> : Selector<None, T>
	{
		public static implicit operator Func<T>(ResultSelector<T> instance) => instance.Get().ToResult().ToDelegate();

		public ResultSelector(ISelect<None, T> subject) : base(subject) {}
	}

	public class ResultInstanceSelector<_, T> : Selector<_, IResult<T>>
	{
		public ResultInstanceSelector(ISelect<_, IResult<T>> subject) : base(subject) {}

		public Selector<_, T> Value() => Select(Results<T>.Default);

		public Selector<_, Func<T>> Delegate() => Select(DelegateSelector<T>.Default);
	}

	public class ExpressionSelector<_, T> : Selector<_, Expression<T>>
	{
		public ExpressionSelector(ISelect<_, Expression<T>> subject) : base(subject) {}

		public Selector<_, T> Compile() => Select(Runtime.Invocation.Expressions.Compiler<T>.Default);
	}

	public class ExpressionSelector<_> : Selector<_, Expression>
	{
		public ExpressionSelector(ISelect<_, Expression> subject) : base(subject) {}

		public ExpressionSelector<_, TTo> Select<TTo>(ISelect<Expression, Expression<TTo>> select)
			=> Select(select.Get);

		public ExpressionSelector<_, TTo> Select<TTo>(Func<Expression, Expression<TTo>> select)
			=> new ExpressionSelector<_, TTo>(Get().Select(select));
	}

	public class TypeSelector<T> : Selector<T, Type>
	{
		public TypeSelector(ISelect<T, Type> subject) : base(subject) {}

		public Selector<T, TypeInfo> Metadata() => Select(TypeMetadata.Default);
	}

	public class SelectionSelector<_, TIn, TOut> : Selector<_, ISelect<TIn, TOut>>
	{
		public SelectionSelector(ISelect<_, ISelect<TIn, TOut>> subject) : base(subject) {}

		public Selector<_, Func<TIn, TOut>> Delegate() => Select(DelegateSelector<TIn, TOut>.Default);
	}

	public class CommandInstanceSelector<TIn, T> : Selector<TIn, ICommand<T>>
	{
		public CommandInstanceSelector(ISelect<TIn, ICommand<T>> subject) : base(subject) {}

		public IAssign<TIn, T> ToAssignment() => new SelectedAssignment<TIn, T>(Get().Get);
	}

	public class CommandSelector : CommandSelector<None>
	{
		public static implicit operator System.Action(CommandSelector instance) => instance.Get().ToDelegate();

		public CommandSelector(ICommand command) : base(command) => Command = command;

		public ICommand Command { get; }

		public CommandSelector<object> Any() => new CommandSelector<object>(new Any(Get()));
	}


	sealed class Any : ICommand<object>
	{
		readonly ICommand<None> _command;

		public Any(ICommand<None> command) => _command = command;

		public void Execute(object _)
		{
			_command.Execute();
		}
	}

	public class CommandSelector<T> : Instance<ICommand<T>>
	{
		public static implicit operator System.Action<T>(CommandSelector<T> instance) => instance.Get().ToDelegate();

		public CommandSelector(ICommand<T> command) : base(command) {}

		public CommandSelector Input(T parameter = default)
			=> new CommandSelector(new FixedParameterCommand<T>(Get().Execute, parameter));

		public CommandSelector Input(IResult<T> parameter)
			=> new CommandSelector(new DelegatedParameterCommand<T>(Get().Execute, parameter.Get));

		public CommandSelector<T> And(params ICommand<T>[] commands)
			=> new CommandSelector<T>(new CompositeCommand<T>(Get().Yield().Concat(commands).ToImmutableArray()));

		/*public static IAny Clear<T>(this ICommand<T> @this) => @this.Then().ToCommand(default(T)).Any();*/

		/**/

		public Selector<T, None> ToSelector() => new Selector<T, None>(Get().ToSelect());

		public AlterationSelector<T> ToConfiguration()
			=> new AlterationSelector<T>(new ConfiguringAlteration<T>(Get()));
	}

	public class AlterationSelector<T> : Selector<T, T>
	{
		public static implicit operator Func<T, T>(AlterationSelector<T> instance) => instance.Get().ToDelegate();

		public AlterationSelector(IAlteration<T> subject) : base(subject) {}
	}

	public class ConditionSelector<T> : Selector<T, bool>
	{
		public ConditionSelector(ISelect<T, bool> subject) : base(subject) {}

		public ConditionSelector<T> Or(params Func<T, bool>[] others)
			=> Or(others.Select(x => new Condition<T>(x)).Open<ISelect<T, bool>>());

		public ConditionSelector<T> Or(params ISelect<T, bool>[] others)
			=> new ConditionSelector<T>(new AnyCondition<T>(others.Prepend(Get()).Open()));

		public ConditionSelector<T> And(params Func<T, bool>[] others)
			=> And(others.Select(x => new Condition<T>(x)).Open<ISelect<T, bool>>());

		public ConditionSelector<T> And(params ISelect<T, bool>[] others)
			=> new ConditionSelector<T>(new AllCondition<T>(others.Prepend(Get()).Open()));

		public ConditionSelector<T> Inverse()
			=> new ConditionSelector<T>(InverseSpecifications<T>.Default.Get(new Condition<T>(Get().Get)));
	}

	public class Selector<_, T> : IResult<ISelect<_, T>>, IActivateUsing<ISelect<_, T>>
	{
		public static implicit operator Func<_, T>(Selector<_, T> instance) => instance.Get().ToDelegate();

		readonly ISelect<_, T> _subject;

		public Selector(ISelect<_, T> subject) => _subject = subject;

		public ISelect<_, T> Get() => _subject;

		public TypeSelector<_> Type() => new TypeSelector<_>(_subject.Select(InstanceType<T>.Default));

		public Selector<_, TTo> Activate<TTo>() where TTo : IActivateUsing<T> => Select(Activations<T, TTo>.Default);

		public Selector<_, TTo> Select<TTo>(ISelect<T, TTo> select) => Select(select.Get);

		public Selector<_, TTo> Select<TTo>(Func<T, TTo> select) => new Selection<_, T, TTo>(Get().Get, select).Then();

		public Selector<_, T> Assigned() => Get().If(A.Of<IsAssigned<_>>()).Then();

		public Selector<_, TTo> Cast<TTo>() => Select(CastOrDefault<T, TTo>.Default);

		public Selector<_, TTo> CastForResult<TTo>() => Select(ResultAwareCast<T, TTo>.Default);

		public Selector<_, T> OnceStriped() => new Selector<_, T>(OncePerParameter<_, T>.Default.Get(_subject));

		public Selector<_, T> OnlyOnce() => new Selector<_, T>(OnlyOnceAlteration<_, T>.Default.Get(_subject));

		public Selector<_, T> Protect() => new Selector<_, T>(ProtectAlteration<_, T>.Default.Get(_subject));

		public Selector<_, T> Stripe() => new Selector<_, T>(StripedAlteration<_, T>.Default.Get(_subject));

		public Selector<_, T> Try<TException>() where TException : Exception
			=> new Selector<_, T>(new Try<TException, _, T>(Get().Get));
	}
}