using System.Collections.Generic;
using Super.Model.Commands;
using Super.Model.Sequences.Collections.Commands;

namespace Super.Model.Sequences.Collections
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