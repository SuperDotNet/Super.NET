using Super.Model.Selection;

namespace Super.Model.Results
{
	sealed class Results<T> : Select<IResult<T>, T>
	{
		public static Results<T> Default { get; } = new Results<T>();

		Results() : base(x =>
		                 {
			                 var asdf = x.Get();
			                 return asdf;
		                 }) {}
	}
}