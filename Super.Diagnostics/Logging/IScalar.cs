using System.Collections.Generic;

namespace Super.Diagnostics.Logging
{
	public interface IScalar : IReadOnlyDictionary<string, ScalarProperty> {}
}