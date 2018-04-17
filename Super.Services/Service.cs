using Super.ExtensionMethods;
using System;
using Super.Model.Selection;

namespace Super.Services
{
	public sealed class Service<T> : DecoratedSelect<Uri, T>
	{
		public static Service<T> Default { get; } = new Service<T>();

		Service() : base(ClientStore.Default.Out(Api<T>.Default)) {}
	}
}