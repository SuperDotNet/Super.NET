using System.Collections.Immutable;

namespace Super.Application.Console
{
	public interface IApplicationContexts : IApplicationContexts<ImmutableArray<string>, IApplicationContext<ImmutableArray<string>>> {}
}