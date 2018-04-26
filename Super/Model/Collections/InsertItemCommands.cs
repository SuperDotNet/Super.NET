using Super.Model.Commands;
using Super.Model.Selection;
using Super.Model.Specifications;
using System;
using System.Collections.Generic;

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
			=> _specification.If(new InsertItemCommand<T>(parameter.Parameter, _index).Select(),
			                     parameter.Result.Select())
			                 .ToCommand();
	}
}