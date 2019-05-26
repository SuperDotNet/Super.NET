using System;
using System.Reflection;
using Super.Model.Sequences;
using Super.Reflection;
using Super.Reflection.Selection;
using Super.Runtime.Activation;

namespace Super.Runtime.Environment
{
	public sealed class Types : SystemStore<Array<Type>>, IArray<Type>
	{
		public static Types Default { get; } = new Types();

		Types() : base(Types<PublicAssemblyTypes>.Default) {}
	}

	public sealed class Types<T> : ArrayStore<Type> where T : class, IActivateUsing<Assembly>, IArray<Type>
	{
		public static Types<T> Default { get; } = new Types<T>();

		Types() : base(Assemblies.Default.Query()
		                         .Select(I<T>.Default.From)
		                         .SelectMany(x => x.Get().Open())
		                         .Selector()) {}
	}
}