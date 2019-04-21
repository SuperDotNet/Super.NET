using Super.Model.Commands;
using Super.Model.Results;
using Super.Model.Selection;
using System;

namespace Super.Application
{
	public interface IApplicationContext : IDisposable {}

	public interface IApplicationContext<in T> : ICommand<T>, IApplicationContext {}

	public interface IApplicationContext<in TIn, out TOut> : ISelect<TIn, TOut>, IApplicationContext {}

	public class ApplicationContext<TIn, TOut> : DecoratedSelect<TIn, TOut>,
	                                             IApplicationContext<TIn, TOut>
	{
		readonly IDisposable _disposable;

		public ApplicationContext(ISelect<TIn, TOut> @select, IDisposable disposable) : base(@select)
			=> _disposable = disposable;

		public void Dispose()
		{
			_disposable.Dispose();
		}
	}

	public interface IApplicationContexts<in TIn, out TContext> : ISelect<TIn, TContext>
		where TContext : IApplicationContext {}

	public class ApplicationContext<T> : DecoratedCommand<T>, IApplicationContext<T>
	{
		readonly IDisposable _disposable;

		public ApplicationContext(ICommand<T> command, IDisposable disposable) : base(command)
			=> _disposable = disposable;

		public void Dispose()
		{
			_disposable.Dispose();
		}
	}

	public class ApplicationArgument<T> : Instance<T>
	{
		public ApplicationArgument(T instance) : base(instance) {}
	}

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

	sealed class ServiceSelector<T> : Select<IServiceProvider, T>
	{
		public static ServiceSelector<T> Default { get; } = new ServiceSelector<T>();

		ServiceSelector() : base(x => x.Get<T>()) {}
	}

	public class ApplicationContexts<TIn, TContext> : DecoratedSelect<TIn, IApplicationContext<TIn>>
		where TContext : IApplicationContext<TIn>
	{
		protected ApplicationContexts(ISelect<TIn, IServices> services)
			: base(services.Select(ServiceSelector<TContext>.Default).Then().Cast<IApplicationContext<TIn>>().Get()) {}
	}

	public class ApplicationContexts<TContext, TIn, TOut> : DecoratedSelect<TIn, IApplicationContext<TIn, TOut>>
		where TContext : IApplicationContext<TIn, TOut>
	{
		protected ApplicationContexts(ISelect<TIn, IServices> services)
			: base(services.Select(ServiceSelector<TContext>.Default)
			               .Then()
			               .Cast<IApplicationContext<TIn, TOut>>()
			               .Get()) {}
	}
}