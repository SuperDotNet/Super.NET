using System.Threading.Tasks;
using Super.Model.Selection;

namespace Super.Operations
{
	public sealed class SelectOperation<T> : Select<Task<T>, ValueTask<T>>
	{
		public static SelectOperation<T> Default { get; } = new SelectOperation<T>();

		SelectOperation() : base(x => new ValueTask<T>(x)) {}
	}
}