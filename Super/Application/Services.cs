﻿using Super.Model.Collections;
using Super.Model.Commands;
using Super.Model.Selection;
using Super.Model.Sources;
using Super.Reflection;
using Super.Runtime.Activation;
using System;

namespace Super.Application
{
	public interface IApplicationContext : IDisposable {}

	public interface IApplicationContext<in T> : ICommand<T>, IApplicationContext {}

	public interface IApplicationContext<in TParameter, out TResult> : ISelect<TParameter, TResult>, IApplicationContext {}

	public class ApplicationContext<TParameter, TResult> : DecoratedSelect<TParameter, TResult>,
	                                                       IApplicationContext<TParameter, TResult>
	{
		readonly IDisposable _disposable;

		public ApplicationContext(ISelect<TParameter, TResult> @select, IDisposable disposable) : base(@select)
			=> _disposable = disposable;

		public void Dispose()
		{
			_disposable.Dispose();
		}
	}

	public interface IApplicationContexts<in TParameter, out TContext> : ISelect<TParameter, TContext>
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

	public class ApplicationArgument<T> : Source<T>
	{
		public ApplicationArgument(T instance) : base(instance) {}
	}

	public interface IServices<in T> : ISelect<T, IServices> {}

	public sealed class Services<T> : DecoratedSelect<T, IServices>, IServices<T>, IActivateMarker<IRegistration>
	{
		public static IServices<T> Default { get; } = new Services<T>();

		Services() : this(ServiceRegistration.Default) {}

		public Services(ISource<IRegistration> registration)
			: base(In<T>.Activate<InstanceRegistration<T>>()
			            .Sequence()
			            .Select(new AppendValueSelector<IRegistration>(registration))
			            .Activate(I<CompositeRegistration>.Default)
			            .Select(ServiceOptions.Default.Select(I<Services>.Default).Select)
			            .Cast(I<IServices>.Default)
			            .Select(ServiceConfiguration.Default.Select(x => x.ToConfiguration()))) {}
	}

	sealed class ServiceSelector<T> : Select<IServiceProvider, T>
	{
		public static ServiceSelector<T> Default { get; } = new ServiceSelector<T>();

		ServiceSelector() : base(x => x.Get<T>()) {}
	}

	public class ApplicationContexts<TParameter, TContext> : DecoratedSelect<TParameter, IApplicationContext<TParameter>>
		where TContext : IApplicationContext<TParameter>
	{
		protected ApplicationContexts(ISelect<TParameter, IServices> services)
			: base(services.Select(ServiceSelector<TContext>.Default)
			               .Cast(I<IApplicationContext<TParameter>>.Default)) {}
	}

	public class ApplicationContexts<TContext, TParameter, TResult>
		: DecoratedSelect<TParameter, IApplicationContext<TParameter, TResult>>
		where TContext : IApplicationContext<TParameter, TResult>
	{
		protected ApplicationContexts(ISelect<TParameter, IServices> services)
			: base(services.Select(ServiceSelector<TContext>.Default)
			               .Cast(I<IApplicationContext<TParameter, TResult>>.Default)) {}
	}
}