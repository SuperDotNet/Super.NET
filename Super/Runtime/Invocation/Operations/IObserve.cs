using System.Threading.Tasks;
using Super.Model.Selection;

namespace Super.Runtime.Invocation.Operations
{
	public interface IObserve<T> : ISelect<Task<T>, T> {}
}