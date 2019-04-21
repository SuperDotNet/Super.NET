using Super.Compose;
using Super.Model.Commands;
using Super.Model.Selection.Stores;
using System.Collections.Generic;

namespace Super.Model.Collections.Commands
{
	sealed class ItemCommands<T> : Store<IList<T>, ICommand<T>>
	{
		public static ItemCommands<T> Default { get; } = new ItemCommands<T>();

		ItemCommands() : base(A.Of<AddItemCommands<T>>().Select(A.Of<InsertItemCommands<T>>()).Get) {}
	}
}