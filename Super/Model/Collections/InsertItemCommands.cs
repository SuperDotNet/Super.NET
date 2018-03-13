using System;
using System.Collections.Generic;
using Super.ExtensionMethods;
using Super.Model.Commands;
using Super.Model.Sources;
using Super.Model.Specifications;

namespace Super.Model.Collections
{
	sealed class InsertItemCommands<T> : IDecoration<IList<T>, ICommand<T>>
	{
		public static InsertItemCommands<T> Default { get; } = new InsertItemCommands<T>();

		InsertItemCommands() : this(DeclaredGroupIndexes<T>.Default, DeclaredGroupIndexes<T>.Default.Get) {}

		readonly Func<T, int> _index;

		readonly ISpecification<T> _specification;

		public InsertItemCommands(ISpecification<T> specification, Func<T, int> index)
		{
			_specification = specification;
			_index         = index;
		}

		public ICommand<T> Get(Decoration<IList<T>, ICommand<T>> parameter)
			=> parameter.Result.Adapt()
			            .Unless(_specification, new InsertItemCommand<T>(parameter.Parameter, _index).Adapt())
			            .ToCommand();
	}
}