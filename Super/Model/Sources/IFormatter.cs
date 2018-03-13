namespace Super.Model.Sources
{
	public interface IFormatter<in T> : ISource<T, string> {}
}