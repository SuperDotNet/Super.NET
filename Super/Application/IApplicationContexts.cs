using Super.Model.Selection;

namespace Super.Application
{
	public interface IApplicationContexts<in TIn, out TContext> : ISelect<TIn, TContext>
		where TContext : IApplicationContext {}
}