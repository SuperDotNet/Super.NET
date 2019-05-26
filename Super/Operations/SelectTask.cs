using System.Threading.Tasks;
using Super.Model.Selection;

namespace Super.Operations
{
	public sealed class SelectTask<T> : Select<ValueTask<T>, Task<T>>
	{
		public static SelectTask<T> Default { get; } = new SelectTask<T>();

		SelectTask() : base(x => x.AsTask()) {}
	}
}