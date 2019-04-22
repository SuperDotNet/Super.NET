using System.IO;
using Super.Model.Selection.Conditions;

namespace Super.Io
{
	sealed class FilePathExists : Condition<string>
	{
		public static FilePathExists Default { get; } = new FilePathExists();

		FilePathExists() : base(File.Exists) {}
	}
}