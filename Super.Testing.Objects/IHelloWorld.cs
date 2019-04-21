using Super.Model.Selection;

namespace Super.Testing.Objects
{
	public interface IHelloWorld
	{
		string GetMessage();
	}

	public interface IService<T> : ISelect<T, T> {}
}