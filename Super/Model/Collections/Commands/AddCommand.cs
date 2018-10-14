using Super.Model.Commands;
using System.Collections.Generic;

namespace Super.Model.Collections.Commands
{
	class AddCommand<T> : DelegatedCommand<T>
	{
		public AddCommand(ICollection<T> collection) : base(collection.Add) {}
	}
}