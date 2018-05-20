namespace Super.Application.Hosting.BenchmarkDotNet
{
	sealed class AlignJitLoops : ConfigureJob
	{
		public static AlignJitLoops Default { get; } = new AlignJitLoops();

		AlignJitLoops() : base(AlignJitLoopsSetting.Default) {}
	}
}