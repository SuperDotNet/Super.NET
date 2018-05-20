using System.IO;
using Super.Model.Specifications;

namespace Super.Io
{
	sealed class FilePathExists : DelegatedSpecification<string>
	{
		public static FilePathExists Default { get; } = new FilePathExists();

		FilePathExists() : base(File.Exists) {}
	}
}