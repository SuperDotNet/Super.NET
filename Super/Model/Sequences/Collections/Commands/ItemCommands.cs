using System.Collections.Generic;
using Super.Model.Commands;
using Super.Model.Selection.Stores;

namespace Super.Model.Sequences.Collections.Commands
{
	sealed class ItemCommands<T> : Store<IList<T>, ICommand<T>>
	{
		public static ItemCommands<T> Default { get; } = new ItemCommands<T>();

		ItemCommands() : base(AddItemCommands<T>.Default.Select(InsertItemCommands<T>.Default).Get) {}
	}
}