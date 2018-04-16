using Super.Diagnostics.Logging;
using Super.Text;

namespace Super.Runtime.Execution
{
	sealed class DetailsFormatter : IFormatter<Details>
	{
		public static DetailsFormatter Default { get; } = new DetailsFormatter();

		DetailsFormatter() {}

		public string Get(Details parameter)
			=> $"[{parameter.Observed.ToString(TimestampFormat.Default)}] {parameter.Name}";
	}

	/*class Execution<T> : Contextual<IIExecutionContext, T>
	{
		public Execution(IInstance<IIExecutionContext> parameter) : base(parameter) {}

		public Execution(ISource<IIExecutionContext, T> source, IInstance<IIExecutionContext> parameter) : base(source, parameter) {}
	}

	class Contextual<TContext, T> : DelegatedParameterSource<TContext, T> where TContext : class
	{
		public Contextual(IInstance<TContext> parameter) : this(null, parameter) {}

		public Contextual(ISource<TContext, T> source, IInstance<TContext> parameter) : base(source, parameter) {}
	}*/
}