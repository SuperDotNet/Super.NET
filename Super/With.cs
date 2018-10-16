using Super.Model.Selection;
using Super.Model.Sources;
using Super.Reflection;
using Super.Reflection.Types;
using Super.Runtime.Activation;
using System;

namespace Super
{
	public sealed class With<T>
	{
		public static With<T> Instance { get; } = new With<T>();

		With() {}

		public ISource<T> Default() => Model.Sources.Default<T>.Instance;

		public ISource<T> A(T instance) => new Source<T>(instance);

		public ISource<T> A(Func<T> select) => new DelegatedSource<T>(select);

		public ISelect<TAccept, T> Accept<TAccept>() => Activator<T>.Default.Out(I<TAccept>.Default);
	}

	public sealed class For<T>
	{
		public static For<T> Default { get; } = new For<T>();

		For() {}

		public IGeneric<T> A(Type definition) => new Generic<T>(definition);
	}
}