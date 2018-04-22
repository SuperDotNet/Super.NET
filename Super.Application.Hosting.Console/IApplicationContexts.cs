using System.Collections.Immutable;

namespace Super.Application.Hosting.Console
{
	public interface IApplicationContexts : IApplicationContexts<ImmutableArray<string>, IApplicationContext<ImmutableArray<string>>> {}
}