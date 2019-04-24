using Super.Model.Collections.Commands;
using Super.Model.Commands;
using System.Collections.Generic;

namespace Super.Model.Collections
{
	public class Membership<T> : IMembership<T>
	{
		public Membership(ICollection<T> collection)
			: this(new AddCommand<T>(collection), new RemoveCommand<T>(collection)) {}

		public Membership(ICommand<T> add, ICommand<T> remove)
		{
			Add    = add;
			Remove = remove;
		}

		public ICommand<T> Add { get; }

		public ICommand<T> Remove { get; }
	}
}