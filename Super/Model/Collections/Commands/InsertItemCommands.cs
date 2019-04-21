using Super.Model.Collections.Groups;
using Super.Model.Commands;
using Super.Model.Selection;
using Super.Model.Selection.Conditions;
using System;
using System.Collections.Generic;

namespace Super.Model.Collections.Commands
{
	sealed class InsertItemCommands<T> : IDecoration<IList<T>, ICommand<T>>
	{
		public static InsertItemCommands<T> Default { get; } = new InsertItemCommands<T>();

		InsertItemCommands() : this(DeclaredGroupIndexes<T>.Default.Condition,
		                            DeclaredGroupIndexes<T>.Default.Get) {}

		readonly Func<T, int> _index;

		readonly ICondition<T> _condition;

		public InsertItemCommands(ICondition<T> condition, Func<T, int> index)
		{
			_condition = condition;
			_index     = index;
		}

		public ICommand<T> Get(Decoration<IList<T>, ICommand<T>> parameter)
			=> parameter.Result
			            .ToSelect()
			            .Unless(_condition, new InsertItemCommand<T>(parameter.Parameter, _index).ToSelect())
			            .ToCommand();
	}
}