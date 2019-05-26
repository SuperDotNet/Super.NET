using System.Reflection;
using Super.Model.Selection;
using Super.Model.Sequences;

namespace Super.Reflection.Members
{
	sealed class Parameters : Select<ConstructorInfo, Array<ParameterInfo>>
	{
		public static Parameters Default { get; } = new Parameters();

		Parameters() : base(x => x.GetParameters()) {}
	}
}