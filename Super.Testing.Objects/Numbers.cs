using Super.Model.Sequences;
using Super.Reflection;

namespace Super.Testing.Objects
{
	sealed class Numbers : ArrayStore<uint, int>
	{
		public static Numbers Default { get; } = new Numbers();

		Numbers() : base(AllNumbers.Default.ToDelegate().To(I<ClassicTake<int>>.Default).Result().Get) {}
	}
}