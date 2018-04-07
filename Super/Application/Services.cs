using Super.ExtensionMethods;
using Super.Model.Collections;
using Super.Model.Commands;
using Super.Model.Containers;
using Super.Model.Instances;
using Super.Model.Sources;
using Super.Reflection;
using Super.Runtime.Activation;
using System;

namespace Super.Application
{
	public interface IApplicationContext : IDisposable {}

	public interface IApplicationContext<in T> : ICommand<T>, IApplicationContext {}

	public interface IApplicationContext<in TParameter, out TResult> : ISource<TParameter, TResult>, IApplicationContext {}

	public class ApplicationContext<TParameter, TResult> : DecoratedSource<TParameter, TResult>,
	                                                       IApplicationContext<TParameter, TResult>
	{
		readonly IDisposable _disposable;

		public ApplicationContext(ISource<TParameter, TResult> source, IDisposable disposable) : base(source)
			=> _disposable = disposable;

		public void Dispose()
		{
			_disposable.Dispose();
		}
	}

	public interface IApplicationContexts<in TParameter, out TContext> : ISource<TParameter, TContext>
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

	public interface IServices<in T> : ISource<T, IServices> {}

	public sealed class Services<T> : DecoratedSource<T, IServices>, IServices<T>, IActivateMarker<IRegistration>
	{
		public static IServices<T> Default { get; } = new Services<T>();

		Services() : this(ServiceRegistration.Default) {}

		public Services(IInstance<IRegistration> registration)
			: base(Activate.New<T, InstanceRegistration<T>>()
			               .Out(YieldCoercer<IRegistration>.Default)
			               .Out(new AppendInstanceCoercer<IRegistration>(registration))
			               .Out(New<CompositeRegistration>.Default)
			               .Out(ServiceOptions.Default.Select(I<Services>.Default).Get)
			               .Out(Cast<IServices>.Default)
			               .Out(ServiceConfiguration.Default.ToCommand().ToConfiguration())) {}
	}

	sealed class ServiceCoercer<T> : ISource<IServiceProvider, T>
	{
		public static ServiceCoercer<T> Default { get; } = new ServiceCoercer<T>();

		ServiceCoercer() {}

		public T Get(IServiceProvider parameter) => parameter.Get<T>();
	}

	public class ApplicationContexts<TParameter, TContext> : DecoratedSource<TParameter, IApplicationContext<TParameter>>
		where TContext : IApplicationContext<TParameter>
	{
		protected ApplicationContexts(ISource<TParameter, IServices> services)
			: base(services.Out(ServiceCoercer<TContext>.Default)
			               .Out(Cast<IApplicationContext<TParameter>>.Default)) {}
	}

	public class ApplicationContexts<TContext, TParameter, TResult>
		: DecoratedSource<TParameter, IApplicationContext<TParameter, TResult>>
		where TContext : IApplicationContext<TParameter, TResult>
	{
		protected ApplicationContexts(ISource<TParameter, IServices> services)
			: base(services.Out(ServiceCoercer<TContext>.Default)
			               .Out(Cast<IApplicationContext<TParameter, TResult>>.Default)) {}
	}
}