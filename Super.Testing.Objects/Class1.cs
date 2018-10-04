using AutoFixture;
using JetBrains.Annotations;
using Serilog;
using Super.Diagnostics.Logging;
using Super.Model.Collections;
using Super.Model.Selection;
using Super.Model.Selection.Stores;
using Super.Model.Sequences;
using Super.Model.Sources;
using Super.Runtime.Activation;
using Super.Text;
using Super.Text.Formatting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive;

namespace Super.Testing.Objects
{
	sealed class NativeArrays : IArrays<int>
	{
		public static NativeArrays Default { get; } = new NativeArrays();

		NativeArrays() : this(Select.Default, Data.Default) {}

		readonly Func<string, int> _select;
		readonly string[]          _data;

		public NativeArrays(Func<string, int> select, string[] data)
		{
			_select = select;
			_data   = data;
		}

		public ReadOnlyMemory<int> Get() => _data.Select(_select).Where(x => x > 0).ToArray();
	}

	sealed class Chain : IArrays<int>
	{
		public static Chain Default { get; } = new Chain();

		Chain() : this(new Selector<string, int>(Select.Default).Where(x => x > 0), View.Default) {}

		readonly ISelect<ReadOnlyMemory<string>, ReadOnlyMemory<int>> _direct;
		readonly ReadOnlyMemory<string>                               _view;

		public Chain(ISelect<ReadOnlyMemory<string>, ReadOnlyMemory<int>> direct, ReadOnlyMemory<string> view)
		{
			_direct = direct;
			_view   = view;
		}

		public ReadOnlyMemory<int> Get() => _direct.Get(_view);
	}

	sealed class Combo : IArrays<int>
	{
		public static Combo Default { get; } = new Combo();

		Combo() : this(new SelectWhere<string, int>(Select.Default, x => x > 0), View.Default) {}

		readonly ISelect<ReadOnlyMemory<string>, ReadOnlyMemory<int>> _direct;
		readonly ReadOnlyMemory<string>                               _view;

		public Combo(ISelect<ReadOnlyMemory<string>, ReadOnlyMemory<int>> direct, ReadOnlyMemory<string> view)
		{
			_direct = direct;
			_view   = view;
		}

		public ReadOnlyMemory<int> Get() => _direct.Get(_view);
	}

	sealed class View : Arrays<string>
	{
		public static View Default { get; } = new View();

		View() : base(Data.Default) {}
	}

	sealed class AllNumbers : Source<IEnumerable<int>>
	{
		public static ISource<IEnumerable<int>> Default { get; } = new AllNumbers();

		AllNumbers() : base(Enumerable.Range(0, int.MaxValue)) {}
	}

	sealed class Numbers : Store<uint, int[]>
	{
		public static Numbers Default { get; } = new Numbers();

		Numbers() : base(AllNumbers.Default.Take().Capture().Get) {}
	}

	sealed class Data : Source<string[]>
	{
		public static Data Default { get; } = new Data();

		Data() : base(FixtureInstance.Default.Many<string>(10_000)
		                             .Out()
		                             .AsSelect()
		                             .Result()
		                             .Get(Unit.Default)) {}
	}

	public sealed class Sequencing<T>
	{
		public static Sequencing<T> Default { get; } = new Sequencing<T>();

		Sequencing() : this(In<T[]>.Start().Sequence()) {}

		public Sequencing(Model.Sequences.ISequence<T[], T> sequence)
			: this(sequence, Objects.Near.Default, Objects.Far.Default) {}

		public Sequencing(Model.Sequences.ISequence<T[], T> sequence, Selection near, Selection far)
			: this(sequence.Get(), sequence.Select(near).Get(), sequence.Select(far).Get()) {}

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

	sealed class Near : Source<Selection>
	{
		public static Near Default { get; } = new Near();

		Near() : base(new Selection(300, 100)) {}
	}

	sealed class Far : Source<Selection>
	{
		public static Far Default { get; } = new Far();

		Far() : base(new Selection(5000, 300)) {}
	}

	public static class Extensions
	{
		public static ISource<IEnumerable<T>> Many<T>(this ISource<IFixture> @this, uint count)
			=> @this.Select(new Many<T>(count));
	}

	sealed class FixtureInstance : Source<IFixture>
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

	sealed class ArrayEnumerations<T> : Enumerations<T>, IActivateMarker<uint>, IActivateMarker<IEnumerable<T>>
	{
		[UsedImplicitly]
		public ArrayEnumerations(uint count) : this(FixtureInstance.Default.Select(new Many<T>(count)).Get()) {}

		public ArrayEnumerations(IEnumerable<T> source) : base(source.ToArray()) {}
	}

	// ReSharper disable all PossibleMultipleEnumeration
	public class Enumerations<T>
	{
		public Enumerations(uint count) : this(FixtureInstance.Default.Select(new Many<T>(count)).Get()) {}

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

	sealed class Strings : Source<IEnumerable<string>>
	{
		public static Strings Default { get; } = new Strings();

		Strings() : base(new Fixture().CreateMany<string>()) {}
	}

	sealed class Select : Source<Func<string, int>>
	{
		public static Select Default { get; } = new Select();

		Select() : this(x => default) {}

		public Select(Expression<Func<string, int>> instance) : base(instance.Compile()) {}
	}

	sealed class Convert : Source<Converter<string, int>>
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

	sealed class ApplicationDomainFormatter : TextSelect<AppDomain, string>, ISelectFormatter<AppDomain>
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

	sealed class Template : LogMessage<AppDomain>
	{
		public Template(ILogger logger) : base(logger, "Hello World: {@AppDomain:F}") {}
	}
}