using System.Collections.Immutable;

namespace Super.Application.Host.Console
{
	public interface IApplicationContexts : IApplicationContexts<ImmutableArray<string>, IApplicationContext<ImmutableArray<string>>> {}
}