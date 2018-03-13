namespace Super.Model.Sources
{
	public interface IMessage<in T> : ISource<T, string> {}
}