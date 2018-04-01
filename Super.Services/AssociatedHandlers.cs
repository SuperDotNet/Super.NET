using System.Net.Http;
using Super.Model.Sources.Tables;

namespace Super.Services
{
	sealed class AssociatedHandlers : DecoratedTable<HttpClient, System.Net.Http.HttpClientHandler>
	{
		public static AssociatedHandlers Default { get; } = new AssociatedHandlers();

		AssociatedHandlers() :
			base(ReferenceTables<HttpClient, System.Net.Http.HttpClientHandler>.Default.Get(client => null)) {}
	}
}