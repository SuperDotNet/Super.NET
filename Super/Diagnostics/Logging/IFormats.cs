using System.Collections.Immutable;
using Super.Model.Selection;
using Super.Model.Selection.Sources;

namespace Super.Diagnostics.Logging
{
	public interface IFormats : ISelect<string, string>, ISource<ImmutableArray<string>> {}
}