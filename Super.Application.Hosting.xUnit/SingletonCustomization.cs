using AutoFixture.Kernel;

namespace Super.Application.Hosting.xUnit
{
	sealed class SingletonCustomization : InsertCustomization
	{
		public static SingletonCustomization Default { get; } = new SingletonCustomization();

		SingletonCustomization() : base(new MethodInvoker(SingletonQuery.Default)) {}
	}
}