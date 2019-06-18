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

	sealed class Declaration : ContentInstruction
	{
		public static Declaration Default { get; } = new Declaration();

		Declaration() : base($@"<?xml version=""1.0""?>{Environment.NewLine}") {}
	}

	sealed class OpenElement : ContentInstruction
	{
		public OpenElement(string name) : this(Encoder.Default.Get(name)) {}

		public OpenElement(IEnumerable<byte> name) : base(name.Prepend(Open.Default)
		                                                      .Append(Close.Default)
		                                                      .ToArray()) {}
	}

	sealed class CloseElement : ContentInstruction
	{
		public CloseElement(string name) : this(Encoder.Default.Get(name)) {}

		public CloseElement(IEnumerable<byte> name) : base(name.Prepend(Complete.Default)
		                                                       .Prepend(Open.Default)
		                                                       .Append(Close.Default)
		                                                       .ToArray()) {}
	}

	class XmlElementInstruction<T> : ElementInstruction<T>
	{
		public XmlElementInstruction(string name, IInstruction<T> content)
			: base(new OpenElement(name), content, new CloseElement(name)) {}
	}

	/*public class XmlDocumentEmitter<T> : ICompose<T>
	{
		readonly ICompose    _declaration;
		readonly ICompose<T> _content;

		public XmlDocumentEmitter(ICompose declaration, ICompose<T> content)
		{
			_declaration = declaration;
			_content     = content;
		}

		public Composition Get(Composition<T> parameter)
			=> _content.Get(_declaration.Get(parameter).Introduce(parameter.Instance));
	}*/
}