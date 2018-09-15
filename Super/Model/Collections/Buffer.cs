namespace Super.Model.Collections
{
	public interface IEnhancedSelect<TIn, out TOut>
	{
		TOut Get(in TIn parameter);
	}

	public interface IEnhancedCommand<T>
	{
		void Execute(in T parameter);
	}
}