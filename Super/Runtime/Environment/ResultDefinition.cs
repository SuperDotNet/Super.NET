using Super.Model.Results;
using Super.Reflection.Types;

namespace Super.Runtime.Environment
{
	sealed class ResultDefinition : MakeGenericType
	{
		public static ResultDefinition Default { get; } = new ResultDefinition();

		ResultDefinition() : base(typeof(IResult<>)) {}
	}
}