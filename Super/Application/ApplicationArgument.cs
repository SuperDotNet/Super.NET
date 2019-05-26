using Super.Model.Results;

namespace Super.Application
{
	public class ApplicationArgument<T> : Instance<T>
	{
		public ApplicationArgument(T instance) : base(instance) {}
	}
}