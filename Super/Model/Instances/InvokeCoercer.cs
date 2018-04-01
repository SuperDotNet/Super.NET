using Super.ExtensionMethods;
using Super.Model.Sources;
using System.Reactive;

namespace Super.Model.Instances
{
	sealed class InvokeCoercer<T> : DelegatedSource<ISource<Unit, T>, T>
	{
		public static InvokeCoercer<T> Default { get; } = new InvokeCoercer<T>();

		InvokeCoercer() : base(x => x.Get()) {}
	}
}