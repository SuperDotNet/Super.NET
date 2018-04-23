namespace Super.Model.Sources
{
	public class DecoratedSource<T> : DelegatedSource<T>
	{
		public DecoratedSource(ISource<T> source) : base(source.Get) {}
	}
}