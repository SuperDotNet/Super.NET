using AutoFixture;
using JetBrains.Annotations;
using Super.Compose;
using Super.Model.Results;
using Super.Model.Selection;
using Super.Model.Selection.Adapters;
using Super.Model.Sequences;
using Super.Reflection;
using Super.Runtime;
using Super.Runtime.Activation;
using Super.Text;
using Super.Text.Formatting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Super.Testing.Objects
{
	sealed class NativeArray : IArray<int>
	{
		public static NativeArray Default { get; } = new NativeArray();

		NativeArray() : this(Select.Default, Data.Default) {}

		readonly Func<string, int> _select;
		readonly string[]          _data;

		public NativeArray(Func<string, int> select, string[] data)
		{
			_select = select;
			_data   = data;
		}

		public Array<int> Get() => _data.Select(_select).Where(x => x > 0).ToArray();
	}

	sealed class View : ArrayInstance<string>
	{
		public static View Default { get; } = new View();

		View() : base(Data.Default) {}
	}

	sealed class AllNumbers : Instance<IEnumerable<int>>
	{
		public static IResult<IEnumerable<int>> Default { get; } = new AllNumbers();

		AllNumbers() : base(Enumerable.Range(0, int.MaxValue)) {}
	}

	sealed class Numbers : ArrayStore<uint, int>
	{
		public static Numbers Default { get; } = new Numbers();

		Numbers() : base(AllNumbers.Default.ToDelegate().To(I<ClassicTake<int>>.Default).Result().Get) {}
	}

	sealed class ClassicTake<T> : ISelect<uint, IEnumerable<T>>, IActivateUsing<Func<IEnumerable<T>>>
	{
		readonly Func<IEnumerable<T>> _source;

		public ClassicTake(Func<IEnumerable<T>> source) => _source = source;

		public IEnumerable<T> Get(uint parameter) => _source().Take((int)parameter);
	}

	sealed class Data : Instance<string[]>
	{
		public static Data Default { get; } = new Data();

		Data() : base(FixtureInstance.Default.Many<string>(10_000)
		                             .Result()
		                             .Get()) {}
	}

	public sealed class Sequencing<T>
	{
		public static Sequencing<T> Default { get; } = new Sequencing<T>();

		Sequencing() : this(Start.A.Selection<T>().As.Sequence.Array.By.Self.Query()) {}

		public Sequencing(Query<T[], T> sequence) : this(sequence, Objects.Near.Default, Objects.Far.Default) {}

		public Sequencing(Query<T[], T> sequence, Selection near, Selection far)
			: this(sequence.Out(), sequence.Select(near).Out(), sequence.Select(far).Out()) {}

		public Sequencing(ISelect<T[], T[]> full, ISelect<T[], T[]> near, ISelect<T[], T[]> far)
		{
			Full = full;
			Near = near;
			Far  = far;
		}

		public ISelect<T[], T[]> Full { get; }
		public ISelect<T[], T[]> Near { get; }
		public ISelect<T[], T[]> Far { get; }

		public Sequencing<T> Get(ISelect<T[], T[]> select)
			=> new Sequencing<T>(Full.Select(select), Near.Select(select), Far.Select(select));
	}

	sealed class Near : Instance<Selection>
	{
		public static Near Default { get; } = new Near();

		Near() : base(new Selection(300, 100)) {}
	}

	sealed class Far : Instance<Selection>
	{
		public static Far Default { get; } = new Far();

		Far() : base(new Selection(5000, 300)) {}
	}

	public static class Extensions
	{
		public static ISelect<None, IEnumerable<T>> Many<T>(this IResult<IFixture> @this, uint count)
			=> @this.ToSelect().Select(new Many<T>(count));
	}

	sealed class FixtureInstance : Instance<IFixture>
	{
		public static FixtureInstance Default { get; } = new FixtureInstance();

		FixtureInstance() : base(new Fixture()) {}
	}

	sealed class Many<T> : ISelect<IFixture, IEnumerable<T>>
	{
		readonly int _count;

		public Many(uint count) => _count = (int)count;

		public IEnumerable<T> Get(IFixture parameter) => parameter.CreateMany<T>(_count);
	}

	sealed class ArrayEnumerations<T> : Enumerations<T>, IActivateUsing<uint>, IActivateUsing<IEnumerable<T>>
	{
		public static ArrayEnumerations<T> Default { get; } = new ArrayEnumerations<T>();

		ArrayEnumerations() : this(10_000u) {}

		[UsedImplicitly]
		public ArrayEnumerations(uint count) : this(FixtureInstance.Default.ToSelect().Select(new Many<T>(count)).Get()) {}

		public ArrayEnumerations(IEnumerable<T> source) : base(source.ToArray()) {}
	}

	// ReSharper disable all PossibleMultipleEnumeration
	public class Enumerations<T>
	{
		public Enumerations(uint count) : this(FixtureInstance.Default.ToSelect().Select(new Many<T>(count)).Get()) {}

		public Enumerations(IEnumerable<T> source)
			: this(source, Objects.Near.Default, Objects.Far.Default) {}

		public Enumerations(IEnumerable<T> source, Selection near, Selection far)
			: this(source,
			       source.Skip((int)near.Start).Take((int)near.Length.Instance),
			       source.Skip((int)far.Start).Take((int)far.Length.Instance)) {}

		public Enumerations(IEnumerable<T> full, IEnumerable<T> near, IEnumerable<T> far)
		{
			Full = full;
			Near = near;
			Far  = far;
		}

		public IEnumerable<T> Full { get; }
		public IEnumerable<T> Near { get; }
		public IEnumerable<T> Far { get; }

		public Enumerations<T> Get(Func<IEnumerable<T>, IEnumerable<T>> select)
			=> new Enumerations<T>(select(Full), select(Near), select(Far));
	}

	sealed class Strings : Instance<IEnumerable<string>>
	{
		public static Strings Default { get; } = new Strings();

		Strings() : base(new Fixture().CreateMany<string>()) {}
	}

	sealed class Select : Instance<Func<string, int>>
	{
		public static Select Default { get; } = new Select();

		Select() : this(x => default) {}

		public Select(Expression<Func<string, int>> instance) : base(instance.Compile()) {}
	}

	sealed class Convert : Instance<Converter<string, int>>
	{
		public static Convert Default { get; } = new Convert();

		Convert() : this(x => default) {}

		public Convert(Expression<Converter<string, int>> instance) : base(instance.Compile()) {}
	}

	sealed class ApplicationDomainName : FormatEntry<AppDomain>
	{
		public static ApplicationDomainName Default { get; } = new ApplicationDomainName();

		ApplicationDomainName() : base("F", x => x.FriendlyName) {}
	}

	sealed class ApplicationDomainIdentifier : FormatEntry<AppDomain>
	{
		public static ApplicationDomainIdentifier Default { get; } = new ApplicationDomainIdentifier();

		ApplicationDomainIdentifier() : base("I", x => x.Id.ToString()) {}
	}

	sealed class ApplicationDomainFormatter : Text.Selection<AppDomain, string>, ISelectFormatter<AppDomain>
	{
		public static ApplicationDomainFormatter Default { get; } = new ApplicationDomainFormatter();

		ApplicationDomainFormatter()
			: base(DefaultApplicationDomainFormatter.Default,
			       ApplicationDomainName.Default,
			       ApplicationDomainIdentifier.Default) {}
	}

	sealed class DefaultApplicationDomainFormatter : IFormatter<AppDomain>
	{
		public static DefaultApplicationDomainFormatter Default { get; } = new DefaultApplicationDomainFormatter();

		DefaultApplicationDomainFormatter() {}

		public string Get(AppDomain parameter) => $"AppDomain: {parameter.FriendlyName}";
	}

	/*sealed class Template : LogMessage<AppDomain>
	{
		public Template(ILogger logger) : base(logger, "Hello World: {@AppDomain:F}") {}
	}*/
}