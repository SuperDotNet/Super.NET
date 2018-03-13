using System.Reactive;
using Super.ExtensionMethods;
using Super.Model.Sources;

namespace Super.Model.Instances
{
	sealed class SourceAdapterCoercer<T> : ISource<IInstance<T>, ISource<Unit, T>>
	{
		public static SourceAdapterCoercer<T> Default { get; } = new SourceAdapterCoercer<T>();

		SourceAdapterCoercer() {}

		public ISource<Unit, T> Get(IInstance<T> parameter) => parameter.Adapt();
	}
}