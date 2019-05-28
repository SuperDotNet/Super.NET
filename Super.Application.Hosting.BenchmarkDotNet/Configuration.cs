using BenchmarkDotNet.Configs;
using Super.Compose;
using Super.Compose.Selections;
using Super.Model.Results;
using Super.Model.Selection;
using Super.Model.Sequences;

namespace Super.Application.Hosting.BenchmarkDotNet
{
	public sealed class Configuration : Validated<Array<string>, IConfig>
	{
		public static Configuration Default { get; } = new Configuration();

		Configuration() : this(Start.A.Condition.Of.Type<string>().As.Sequence.Immutable,
		                       Start.A.Selection.Of.Type<string>().As.Sequence.Immutable) {}

		public Configuration(Compose.Conditions.Extent<Array<string>> condition, Extent<Array<string>> selection)
			: base(condition.By.Calling(x => x.Length > 0), selection.By.Returning(Quick.Default.AsDefined()),
			       selection.By.Returning(Deployed.Default.AsDefined())) {}
	}
}