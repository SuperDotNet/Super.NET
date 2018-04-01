using System;
using Super.ExtensionMethods;
using Super.Model.Sources;

namespace Super.Services
{
	public sealed class Service<T> : DecoratedSource<Uri, T>
	{
		public static Service<T> Default { get; } = new Service<T>();

		Service() : base(ClientStore.Default.Out(Api<T>.Default)) {}
	}
}