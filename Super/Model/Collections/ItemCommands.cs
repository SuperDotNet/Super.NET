using Super.ExtensionMethods;
using Super.Model.Commands;
using Super.Model.Selection.Stores;
using System.Collections.Generic;

namespace Super.Model.Collections
{
	sealed class ItemCommands<T> : Store<IList<T>, ICommand<T>>
	{
		public static ItemCommands<T> Default { get; } = new ItemCommands<T>();

		ItemCommands() : base(AddItemCommands<T>.Default.Into(InsertItemCommands<T>.Default).Get) {}
	}
}