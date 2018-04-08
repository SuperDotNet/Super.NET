using Super.Model.Sources;

namespace Super.Runtime.Activation
{
	public interface ISingleton<out T> : ISource<T> {}
}