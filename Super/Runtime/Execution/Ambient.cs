using Super.ExtensionMethods;
using Super.Model.Commands;
using Super.Model.Selection;
using Super.Model.Sources;
using Super.Runtime.Activation;
using System;

namespace Super.Runtime.Execution
{
	static class Ambient
	{
		public static ISource<T> For<T>() where T : class => Activator<T>.Default.To(AmbientAlteration<T>.Default);
	}

	public class Ambient<T> : DecoratedSource<T>, IActivateMarker<ISource<T>>, IDisposable
	{
		readonly Action _dispose;

		public Ambient(Func<T> factory) : this(new DelegatedSource<T>(factory)) {}

		public Ambient(ISource<T> source) : this(source, new Logical<T>()) {}

		public Ambient(ISource<T> source, IMutable<T> mutable) : this(source, mutable, mutable) {}

		public Ambient(ISource<T> source, ISource<T> store, ICommand<T> assign)
			: this(store.Or(source.Select(assign.ToConfiguration())), assign.Select(default).Execute) {}

		public Ambient(ISource<T> source, Action dispose) : base(source) => _dispose = dispose;

		public void Dispose()
		{
			_dispose();
		}
	}
}