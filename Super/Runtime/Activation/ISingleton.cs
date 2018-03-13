using Super.Model.Instances;

namespace Super.Runtime.Activation
{
	public interface ISingleton<out T> : IInstance<T> {}
}