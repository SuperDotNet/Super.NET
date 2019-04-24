using Super.Model.Selection;
using Super.Model.Sequences;
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

		KnownProjectors() : base(ApplicationDomainProjection.Default.Entry()) {}
	}

	public interface IProjectors : ISelect<Type, string, Func<object, IProjection>> {}

	sealed class Projectors : Select<Type, string, Func<object, IProjection>>, IProjectors
	{
		public static Projectors Default { get; } = new Projectors();

		Projectors() : base(KnownProjectors.Default.Select(x => x.Open().ToStore().ToDelegate()).Assume()) {}
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

	class FormattedProjection<T> : Selection<T, IProjection>, IFormattedProjection<T>
	{
		public FormattedProjection(ISelect<T, IProjection> @default, params Pair<string, Func<T, IProjection>>[] pairs)
			: base(@default, pairs) {}
	}

	public class Projection<T> : ISelect<T, IProjection>
	{
		readonly Func<T, string>                      _formatter;
		readonly Func<T, IDictionary<string, object>> _properties;

		public Projection(ISelect<T, string> formatter, params Expression<Func<T, object>>[] expressions)
			: this(formatter.Get, expressions) {}

		public Projection(Func<T, string> formatter, params Expression<Func<T, object>>[] expressions)
			: this(formatter, new Values(expressions.Select(x => new Property<T>(x)).Result<IProperty<T>>())
			                  .Select(x => x.ToOrderedDictionary())
			                  .Get) {}

		public Projection(Func<T, string> formatter, Func<T, IDictionary<string, object>> properties)
		{
			_formatter  = formatter;
			_properties = properties;
		}

		public IProjection Get(T parameter)
			=> new Projection(_formatter(parameter), parameter.GetType(), _properties(parameter));

		sealed class Values : ISelect<T, IEnumerable<Pair<string, object>>>
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

	sealed class Property<T> : Select<T, Pair<string, object>>, IProperty<T>
	{
		public Property(Expression<Func<T, object>> expression)
			: base(expression.Compile().ToSelect().Select(new Pairing(expression.GetMemberInfo().Name))) {}
	}

	sealed class Pairing : Invocation1<string, object, Pair<string, object>>
	{
		public Pairing(string parameter) : base(Pairs.Create, parameter) {}
	}
}