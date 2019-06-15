using System;
using System.Collections.Generic;
using System.Linq;

namespace Super.Serialization.Xml
{
	public sealed class Open : Token
	{
		public static Open Default { get; } = new Open();

		Open() : base('<') {}
	}

	public sealed class Close : Token
	{
		public static Close Default { get; } = new Close();

		Close() : base('>') {}
	}

	public sealed class Complete : Token
	{
		public static Complete Default { get; } = new Complete();

		Complete() : base('/') {}
	}

	sealed class Declaration : Content
	{
		public static Declaration Default { get; } = new Declaration();

		Declaration() : base($@"<?xml version=""1.0""?>{Environment.NewLine}") {}
	}

	sealed class OpenElement : Content
	{
		public OpenElement(string name) : this(Encoder.Default.Get(name)) {}

		public OpenElement(IEnumerable<byte> name) : base(name.Prepend(Open.Default)
		                                                      .Append(Close.Default)
		                                                      .ToArray()) {}
	}

	sealed class CloseElement : Content
	{
		public CloseElement(string name) : this(Encoder.Default.Get(name)) {}

		public CloseElement(IEnumerable<byte> name) : base(name.Prepend(Complete.Default)
		                                                       .Prepend(Open.Default)
		                                                       .Append(Close.Default)
		                                                       .ToArray()) {}
	}

	class XmlElementWriter<T> : ElementWriter<T>
	{
		public XmlElementWriter(string name, IEmit<T> content)
			: base(new Emit(new OpenElement(name)), content, new Emit(new CloseElement(name))) {}
	}

	public class XmlDocumentEmitter<T> : IEmit<T>
	{
		readonly IEmit    _declaration;
		readonly IEmit<T> _content;

		public XmlDocumentEmitter(IEmit declaration, IEmit<T> content)
		{
			_declaration = declaration;
			_content     = content;
		}

		public Composition Get(Composition<T> parameter)
			=> _content.Get(_declaration.Get(parameter).Introduce(parameter.Instance));
	}
}