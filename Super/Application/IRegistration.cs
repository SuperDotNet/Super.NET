using JetBrains.Annotations;
using LightInject;
using Super.ExtensionMethods;
using Super.Model.Collections;
using Super.Model.Commands;
using Super.Model.Instances;
using Super.Model.Sources;
using Super.Model.Sources.Alterations;
using Super.Reflection;
using Super.Runtime.Activation;
using Super.Runtime.Environment;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Super.Application
{
	public interface IServices : IServiceContainer, IServiceProvider {}

	class Services : ServiceContainer, IServices, IActivateMarker<ContainerOptions>
	{
		[UsedImplicitly]
		public Services() {}

		public Services(ContainerOptions options) : base(options) {}

		public object GetService(Type serviceType) => GetInstance(serviceType);
	}

	sealed class ServiceOptions : Component<ContainerOptions>
	{
		public static IInstance<ContainerOptions> Default { get; } = new ServiceOptions();

		ServiceOptions() : base(new ContainerOptions {EnablePropertyInjection = false}) {}
	}

	sealed class ServiceRegistration : Component<IRegistration>
	{
		public static ServiceRegistration Default { get; } = new ServiceRegistration();

		ServiceRegistration() : base(Self<IServiceRegistry>.Default.To(I<DecoratedRegistration>.Default)) {}
	}

	sealed class ServiceConfiguration : Component<ICommand<IServices>>
	{
		public static IInstance<ICommand<IServices>> Default { get; } = new ServiceConfiguration();

		ServiceConfiguration() : base(EmptyCommand<IServices>.Default) {}
	}

	public interface IRegistration : IAlteration<IServiceRegistry> {}

	class DecoratedRegistration : DecoratedAlteration<IServiceRegistry>,
	                              IRegistration,
	                              IActivateMarker<IAlteration<IServiceRegistry>>
	{
		public DecoratedRegistration(ISource<IServiceRegistry, IServiceRegistry> registration) : base(registration) {}
	}

	sealed class InstanceRegistration<T> : IRegistration, IActivateMarker<T>
	{
		readonly T _instance;

		public InstanceRegistration(T instance) => _instance = instance;

		public IServiceRegistry Get(IServiceRegistry parameter)
			=> parameter.RegisterInstance(_instance, _instance.GetType().AssemblyQualifiedName);
	}

	sealed class InstanceRegistration : IRegistration, IActivateMarker<object>
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
		public static RegisterWithDependencies<TFrom, TTo> Default { get; } = new RegisterWithDependencies<TFrom, TTo>();

		RegisterWithDependencies() : base(new ImplementationRegistration(typeof(TFrom), typeof(TTo)),
		                                  RegisterDependencies<TTo>.Default) {}
	}

	sealed class RegisterDependencies<T> : DecoratedRegistration
	{
		public static RegisterDependencies<T> Default { get; } = new RegisterDependencies<T>();

		RegisterDependencies() : base(new RegisterDependencies(typeof(T))) {}
	}

	sealed class GenericTypeDependencySelector : DecoratedAlteration<Type>, IActivateMarker<Type>
	{
		public GenericTypeDependencySelector(Type type)
			: base(IsGenericTypeDefinition.Default
			                              .Adapt()
			                              .Fix(type)
			                              .ToSpecification()
			                              .And(IsConstructedGenericType.Default, IsGenericTypeDefinition.Default.Inverse())
			                              .If(GenericTypeDefinitionAlteration.Default)) {}
	}

	sealed class DependencyCandidates : DecoratedSource<Type, ImmutableArray<Type>>, IActivateMarker<Type>
	{
		public DependencyCandidates(Type type) : base(Constructors.Default
		                                                          .In(TypeMetadataCoercer.Default)
		                                                          .Out(Parameters.Default.Out(x => x.AsEnumerable()))
		                                                          .Out(ParameterType.Default)
		                                                          .Out(type.To(I<GenericTypeDependencySelector>.Default))
		                                                          .Out(IsClass.Default)
		                                                          .Enumerate()) {}
	}

	sealed class NotRegistered : DecoratedSource<IServiceRegistry, Func<Type, bool>>
	{
		public static NotRegistered Default { get; } = new NotRegistered();

		NotRegistered() : base(From<IServiceRegistry>.To(x => x.AvailableServices)
		                                             .Out(x => x.ServiceType)
		                                             .Out(I<NotHave<Type>>.Default)
		                                             .Out(x => x.Adapt().ToDelegate())) {}
	}

	sealed class RegisterDependencies : IRegistration
	{
		readonly ImmutableArray<Type> _candidates;
		readonly Func<IServiceRegistry, Func<Type, bool>> _where;

		public RegisterDependencies(Type type)
			: this(type.To(I<DependencyCandidates>.Default).Get(type), NotRegistered.Default.ToDelegate()) {}

		public RegisterDependencies(ImmutableArray<Type> candidates, Func<IServiceRegistry, Func<Type, bool>> where)
		{
			_candidates = candidates;
			_where = @where;
		}

		public IServiceRegistry Get(IServiceRegistry parameter)
			=> _candidates.Where(_where(parameter))
			              .Aggregate(parameter, (repository, t) => repository.Register(t)
			                                                                 .RegisterDependencies(t));
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

	class CompositeRegistration : IRegistration, IActivateMarker<IEnumerable<IRegistration>>
	{
		readonly ImmutableArray<IRegistration> _configurations;

		public CompositeRegistration(params IRegistration[] configurations) : this(configurations.AsEnumerable()) {}

		public CompositeRegistration(IEnumerable<IRegistration> registrations) : this(registrations.ToImmutableArray()) {}

		public CompositeRegistration(ImmutableArray<IRegistration> configurations) => _configurations = configurations;

		public IServiceRegistry Get(IServiceRegistry parameter) => _configurations.ToArray().Alter(parameter);
	}
}