﻿using AutoFixture;
using Serilog;
using Super.Diagnostics.Logging;
using Super.Model.Collections;
using Super.Model.Selection;
using Super.Model.Selection.Stores;
using Super.Model.Sources;
using Super.Text;
using Super.Text.Formatting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

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

	/*sealed class ExpressionCombo : IArray<int>
	{
		public static ExpressionCombo Default { get; } = new ExpressionCombo();

		ExpressionCombo() : this(new SelectWhereDecorator<string, int>(new ExpressionSelection<string, int>(x => default), x => x > 0), View.Default) {}

		readonly ISelect<ReadOnlyMemory<string>, ReadOnlyMemory<int>> _direct;
		readonly ReadOnlyMemory<string>                               _view;

		public ExpressionCombo(ISelect<ReadOnlyMemory<string>, ReadOnlyMemory<int>> direct, ReadOnlyMemory<string> view)
		{
			_direct = direct;
			_view   = view;
		}

		public ReadOnlyMemory<int> Get() => _direct.Get(_view);
	}*/

	/*sealed class Expression : IArray<int>
	{
		public static Expression Default { get; } = new Expression();

		Expression() : this(new ExpressionSelection<string, int>(x => default).Where(x => x > 0), View.Default) {}

		readonly ISelect<ReadOnlyMemory<string>, ReadOnlyMemory<int>> _direct;
		readonly ReadOnlyMemory<string>                               _view;

		public Expression(ISelect<ReadOnlyMemory<string>, ReadOnlyMemory<int>> direct, ReadOnlyMemory<string> view)
		{
			_direct = direct;
			_view   = view;
		}

		public ReadOnlyMemory<int> Get() => _direct.Get(_view);
	}*/

	sealed class View : Arrays<string>
	{
		public static View Default { get; } = new View();

		View() : base(Data.Default) {}
	}

	sealed class Numbers : Source<IEnumerable<int>>
	{
		public static Numbers Default { get; } = new Numbers();

		Numbers() : base(Enumerable.Range(0, int.MaxValue)) {}
	}

	sealed class Count : Store<uint, int[]>
	{
		public static Count Default { get; } = new Count();

		Count() : base(x => Numbers.Default.Get().Take((int)x).ToArray()) {}
	}

	sealed class Data : Source<string[]>
	{
		public static Data Default { get; } = new Data();

		Data() : base(new Fixture().CreateMany<string>(10_000).ToArray()) {}
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
		public static  Convert Default { get; } = new  Convert();

		Convert() : this(x => default) {}

		public  Convert(Expression<Converter<string, int>> instance) : base(instance.Compile()) {}
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
