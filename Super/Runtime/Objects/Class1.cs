using JetBrains.Annotations;
using Super.Compose;
using Super.Model.Selection;
using Super.Model.Sequences;
using Super.Reflection;
using Super.Runtime.Activation;
using Super.Runtime.Invocation;
using Super.Runtime.Invocation.Expressions;
using Super.Text;
using Super.Text.Formatting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;

namespace Super.Runtime.Objects
{
	public interface IProjection : IReadOnlyDictionary<string, object>
	{
		Type InstanceType { get; }
	}

	public sealed class Projection : ReadOnlyDictionary<string, object>, IProjection
	{
		readonly string _text;

		public Projection(string text, Type instanceType, IDictionary<string, object> properties) : base(properties)
		{
			_text        = text;
			InstanceType = instanceType;
		}

		public Type InstanceType { get; }

		public override string ToString() => _text;
	}

	sealed class KnownProjectors : ArrayInstance<Pair<Type, Func<string, Func<object, IProjection>>>>
	{
		public static KnownProjectors Default { get; } = new KnownProjectors();

		KnownProjectors() : this(ApplicationDomainProjection.Default.Entry()) {}

		public KnownProjectors(params Pair<Type, Func<string, Func<object, IProjection>>>[] items)
			: base(items) {}
	}

	static class Implementations
	{
		public static Func<Type, Func<string, Func<object, IProjection>>> KnownProjectors { get; }
			= A.This(Objects.KnownProjectors.Default)
			   .ToSelect()
			   .Select(x => x.Open().ToStore().ToDelegate())
			   .ToResult()
			   .Emit()
			   .Get;
	}

	public interface IProjectors : ISelect<Type, string, Func<object, IProjection>> {}

	sealed class Projectors : Select<Type, string, Func<object, IProjection>>, IProjectors
	{
		public static Projectors Default { get; } = new Projectors();

		Projectors() : base(Implementations.KnownProjectors) {}
	}

	sealed class ApplicationDomainProjection : FormattedProjection<AppDomain>
	{
		public static ApplicationDomainProjection Default { get; } = new ApplicationDomainProjection();

		ApplicationDomainProjection()
			: base(DefaultApplicationDomainFormatter.Default.Project(x => x.FriendlyName, x => x.Id),
			       ApplicationDomainName.Default.Entry(x => x.FriendlyName, x => x.Id, x => x.IsFullyTrusted),
			       ApplicationDomainIdentifier.Default.Entry(x => x.FriendlyName, x => x.Id, x => x.BaseDirectory,
			                                                 x => x.RelativeSearchPath)) {}
	}

	public interface IFormattedProjection<in T> : ISelect<string, T, IProjection> {}

	class FormattedProjection<T> : TextSelect<T, IProjection>, IFormattedProjection<T>
	{
		public FormattedProjection(ISelect<T, IProjection> @default,
		                           params Pair<string, Func<T, IProjection>>[] pairs)
			: base(@default, pairs) {}
	}

	public class Projection<T> : ISelect<T, IProjection>
	{
		readonly Func<T, string>                      _formatter;
		readonly Func<T, IDictionary<string, object>> _properties;

		public Projection(ISelect<T, string> formatter, params Expression<Func<T, object>>[] expressions)
			: this(formatter.Get, expressions) {}

		public Projection(Func<T, string> formatter, params Expression<Func<T, object>>[] expressions)
			: this(formatter, new Values(expressions.Select(I.A<Property<T>>().From)
			                                        .Result<IProperty<T>>()).Select(x => x.ToOrderedDictionary())
			                                                                .Get) {}

		public Projection(Func<T, string> formatter, Func<T, IDictionary<string, object>> properties)
		{
			_formatter  = formatter;
			_properties = properties;
		}

		public IProjection Get(T parameter)
			=> new Projection(_formatter(parameter), parameter.GetType(), _properties(parameter));

		sealed class Values : ISelect<T, IEnumerable<Pair<string, object>>>, IActivateUsing<Array<IProperty<T>>>
		{
			readonly Array<IProperty<T>> _properties;

			public Values(Array<IProperty<T>> properties) => _properties = properties;

			public IEnumerable<Pair<string, object>> Get(T parameter)
			{
				foreach (var property in _properties.Open())
				{
					yield return property.Get(parameter);
				}
			}
		}
	}

	public interface IProperty<in T> : ISelect<T, Pair<string, object>> {}

	sealed class Property<T> : DecoratedSelect<T, Pair<string, object>>,
	                           IProperty<T>,
	                           IActivateUsing<Expression<Func<T, object>>>
	{
		[UsedImplicitly]
		public Property(Expression<Func<T, object>> expression)
			: this(expression,
			       new Invocation1<string, object, Pair<string, object>>(Pairs.Create,
			                                                             expression.GetMemberInfo().Name)) {}

		[UsedImplicitly]
		public Property(Expression<Func<T, object>> expression, ISelect<object, Pair<string, object>> pairs)
			: base(expression.Compile().ToSelect().Select(pairs)) {}
	}
}