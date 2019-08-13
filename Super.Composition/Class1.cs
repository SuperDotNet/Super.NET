using LightInject;
using Super.Application;
using Super.Compose;
using Super.Model.Commands;
using Super.Model.Selection;
using Super.Model.Selection.Alterations;
using Super.Model.Sequences;
using Super.Model.Sequences.Collections;
using Super.Reflection.Members;
using Super.Reflection.Types;
using Super.Runtime.Activation;
using Super.Runtime.Environment;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Super.Composition
{
	class Services : ServiceContainer, IServices, IActivateUsing<ContainerOptions>
	{
		public Services() {}

		public Services(ContainerOptions options) : base(options) {}

		public object GetService(Type serviceType) => GetInstance(serviceType);
	}

	sealed class ServiceOptions : Component<ContainerOptions>
	{
		public static ServiceOptions Default { get; } = new ServiceOptions();

		ServiceOptions() : base(new ContainerOptions {EnablePropertyInjection = false}.Self) {}
	}

	sealed class ServiceConfiguration : Component<ICommand<IServices>>
	{
		public static ServiceConfiguration Default { get; } = new ServiceConfiguration();

		ServiceConfiguration() : base(EmptyCommand<IServices>.Default.Self) {}
	}

	public interface IRegistration : IAlteration<IServiceRegistry> {}

	class DecoratedRegistration : Alteration<IServiceRegistry>,
	                              IRegistration,
	                              IActivateUsing<IAlteration<IServiceRegistry>>
	{
		public DecoratedRegistration(ISelect<IServiceRegistry, IServiceRegistry> registration) : base(registration) {}
	}

	sealed class InstanceRegistration<T> : IRegistration, IActivateUsing<T>
	{
		readonly T _instance;

		public InstanceRegistration(T instance) => _instance = instance;

		public IServiceRegistry Get(IServiceRegistry parameter)
			=> parameter.RegisterInstance(_instance, _instance.GetType().AssemblyQualifiedName);
	}

	sealed class InstanceRegistration : IRegistration, IActivateUsing<object>
	{
		readonly object _instance;

		public InstanceRegistration(object instance) => _instance = instance;

		public IServiceRegistry Get(IServiceRegistry parameter)
			=> parameter.RegisterInstance(_instance.GetType(), _instance);
	}

	sealed class Registration<TFrom, TTo> : ImplementationRegistration where TTo : class, TFrom
	{
		public static Registration<TFrom, TTo> Default { get; } = new Registration<TFrom, TTo>();

		Registration() : base(typeof(TFrom), typeof(TTo)) {}
	}

	sealed class RegisterWithDependencies<TFrom, TTo> : CompositeRegistration where TTo : class, TFrom
	{
		public static RegisterWithDependencies<TFrom, TTo> Default { get; } =
			new RegisterWithDependencies<TFrom, TTo>();

		RegisterWithDependencies() : base(new ImplementationRegistration(typeof(TFrom), typeof(TTo)),
		                                  RegisterDependencies<TTo>.Default) {}
	}

	sealed class RegisterDependencies<T> : DecoratedRegistration
	{
		public static RegisterDependencies<T> Default { get; } = new RegisterDependencies<T>();

		RegisterDependencies() : base(new RegisterDependencies(typeof(T))) {}
	}

	sealed class RegisterDependencies : IRegistration
	{
		readonly static Func<IServiceRegistry, Func<Type, bool>> Where = CanRegister.Default.Get;

		readonly Array<Type>                              _candidates;
		readonly Func<IServiceRegistry, Func<Type, bool>> _where;

		public RegisterDependencies(Type type) : this(new DependencyCandidates(type).Get(type), Where) {}

		public RegisterDependencies(Array<Type> candidates, Func<IServiceRegistry, Func<Type, bool>> where)
		{
			_candidates = candidates;
			_where      = @where;
		}

		public IServiceRegistry Get(IServiceRegistry parameter)
		{
			return _candidates.Open()
			                  .Where(_where(parameter))
			                  .Aggregate(parameter, (repository, t) => repository.Register(t)
			                                                                     .RegisterDependencies(t));
		}
	}

	sealed class DependencyCandidates : ArrayStore<Type, Type>, IActivateUsing<Type>
	{
		public DependencyCandidates(Type type) : base(A.This(TypeMetadata.Default)
		                                               .Select(Constructors.Default)
		                                               .Query()
		                                               .SelectMany(Parameters.Default.Open())
		                                               .Select(ParameterType.Default)
		                                               .Select(new GenericTypeDependencySelector(type))
		                                               .Where(IsClass.Default)
		                                               .Out()) {}
	}

	class ImplementationRegistration : IRegistration
	{
		readonly Type _from;
		readonly Type _to;

		public ImplementationRegistration(Type @from, Type @to)
		{
			_from = @from;
			_to   = to;
		}

		public IServiceRegistry Get(IServiceRegistry parameter) => parameter.Register(_from, _to);
	}

	sealed class CanRegister : Select<IServiceRegistry, Func<Type, bool>>
	{
		public static CanRegister Default { get; } = new CanRegister();

		CanRegister() : base(Start.A.Selection.Of<IServiceRegistry>()
		                          .By.Calling(x => x.AvailableServices)
		                          .Query()
		                          .Select(ServiceTypeSelector.Default)
		                          .Out()
		                          .Then()
		                          .Activate<NotHave<Type>>()
		                          .Select(DelegateSelector<Type, bool>.Default)) {}
	}

	sealed class ServiceTypeSelector : Select<ServiceRegistration, Type>
	{
		public static ServiceTypeSelector Default { get; } = new ServiceTypeSelector();

		ServiceTypeSelector() : base(x => x.ServiceType) {}
	}

	class Registration : IRegistration
	{
		readonly Type _type;

		public Registration(Type type) => _type = type;

		public IServiceRegistry Get(IServiceRegistry parameter) => parameter.Register(_type);
	}

	sealed class Registration<T> : ImplementationRegistration
	{
		public Registration(Type type) : base(typeof(T), type) {}
	}

	class CompositeRegistration : IRegistration, IActivateUsing<IEnumerable<IRegistration>>
	{
		readonly Array<IRegistration> _configurations;

		public CompositeRegistration(params IRegistration[] configurations) : this(configurations.Result()) {}

		public CompositeRegistration(Array<IRegistration> configurations) => _configurations = configurations;

		public IServiceRegistry Get(IServiceRegistry parameter) => _configurations.Open().Alter(parameter);
	}

	/*
	sealed class ServiceRegistration : Component<IRegistration>
	{
		public static ServiceRegistration Default { get; } = new ServiceRegistration();

		ServiceRegistration() : base(Self<IServiceRegistry>.Default.To(I<DecoratedRegistration>.Default)) {}
	}


	public sealed class Services<T> : DecoratedSelect<T, IServices>, IServices<T>, IActivateUsing<IRegistration>
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