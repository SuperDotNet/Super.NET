using System;
using Super.Model.Selection;

namespace Super.Application
{
	public interface IServices : /*IServiceContainer,*/ IServiceProvider {}

	public interface IServices<in T> : ISelect<T, IServices> {}

	/*public sealed class Services<T> : DecoratedSelect<T, IServices>, IServices<T>, IActivateUsing<IRegistration>
	{
		public static IServices<T> Default { get; } = new Services<T>();

		Services() : this(ServiceRegistration.Default) {}

		public Services(ISource<IRegistration> registration)
			: base(Start.From<T>()
			            .Activate<InstanceRegistration<T>>()
			            .Yield()
			            .Select(new AppendDelegatedValue<IRegistration>(registration))
			            .Select(I<CompositeRegistration>.Default)
			            .Select(ServiceOptions.Default.Select(I<Services>.Default).Select)
			            .Cast(I<IServices>.Default)
			            .Select(ServiceConfiguration.Default.Select(x => x.ToConfiguration()))) {}
	}*/
}