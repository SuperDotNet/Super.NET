using System.Collections.Generic;
using Super.Model.Commands;

namespace Super.Model.Collections
{
	class AddCommand<T> : DelegatedCommand<T>
	{
		public AddCommand(ICollection<T> collection) : base(collection.Add) {}
	}
}