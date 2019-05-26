using System.Collections.Generic;
using Super.Model.Commands;

namespace Super.Model.Sequences.Collections.Commands
{
	class AddCommand<T> : Command<T>
	{
		public AddCommand(ICollection<T> collection) : base(collection.Add) {}
	}
}