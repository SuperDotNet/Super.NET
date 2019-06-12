using System;
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

	sealed class OpenContent : Content
	{
		public OpenContent(string name) : base(Encoder.Default.Get(name)
		                                              .Prepend(Open.Default)
		                                              .Append(Close.Default)
		                                              .ToArray()) {}
	}

	sealed class CloseContent : Content
	{
		public CloseContent(string name) : base(Encoder.Default.Get(name)
		                                               .Prepend(Complete.Default)
		                                               .Prepend(Open.Default)
		                                               .Append(Close.Default)
		                                               .ToArray()) {}
	}

	class XmlElementWriter<T> : ElementWriter<T>
	{
		public XmlElementWriter(string name, IEmit<T> content)
			: base(new Emit(new OpenContent(name)), content, new Emit(new CloseContent(name))) {}
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
		{
			var declaration = _declaration.Get(new Composition(parameter.Output, parameter.Index));
			var result = _content.Get(new Composition<T>(declaration.Output, parameter.Instance, declaration.Index));
			return result;
		}
	}
}