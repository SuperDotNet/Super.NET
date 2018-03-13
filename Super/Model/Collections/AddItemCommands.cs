using System.Collections.Generic;
using Super.Model.Commands;
using Super.Model.Sources;

namespace Super.Model.Collections
{
	sealed class AddItemCommands<T> : DelegatedSource<IList<T>, ICommand<T>>
	{
		public static AddItemCommands<T> Default { get; } = new AddItemCommands<T>();

		AddItemCommands() : base(key => new AddCommand<T>(key)) {}
	}
}