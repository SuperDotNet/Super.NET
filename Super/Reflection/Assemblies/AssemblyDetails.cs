using Super.ExtensionMethods;
using Super.Model.Selection;
using System;
using System.Reflection;

namespace Super.Reflection.Assemblies
{
	sealed class AssemblyDetailsSelector : ISelect<Assembly, AssemblyDetails>
	{
		public static AssemblyDetailsSelector Default { get; } = new AssemblyDetailsSelector();

		AssemblyDetailsSelector() : this(AssemblyTitle.Default.ToDelegate(), AssemblyProduct.Default.ToDelegate(),
		                                 AssemblyCompany.Default.ToDelegate(), AssemblyDescription.Default.ToDelegate(),
		                                 AssemblyConfiguration.Default.ToDelegate(), AssemblyCopyright.Default.ToDelegate(),
		                                 AssemblyVersion.Default.ToDelegate()) {}

		readonly Func<Assembly, string>  _title, _product, _company, _description, _configuration, _copyright;
		readonly Func<Assembly, Version> _version;

		// ReSharper disable once TooManyDependencies -	These are properties taken from the assembly manifest.
		public AssemblyDetailsSelector(Func<Assembly, string> title, Func<Assembly, string> product,
		                               Func<Assembly, string> company, Func<Assembly, string> description,
		                               Func<Assembly, string> configuration, Func<Assembly, string> copyright,
		                               Func<Assembly, Version> version)
		{
			_title         = title;
			_product       = product;
			_company       = company;
			_description   = description;
			_configuration = configuration;
			_copyright     = copyright;
			_version       = version;
		}

		public AssemblyDetails Get(Assembly parameter)
			=> new AssemblyDetails(_title(parameter), _product(parameter), _company(parameter), _description(parameter),
			                       _configuration(parameter), _copyright(parameter), _version(parameter));
	}

	public sealed class AssemblyDetails
	{
		// ReSharper disable once TooManyDependencies -	These are properties taken from the assembly manifest.
		public AssemblyDetails(string title, string product, string company, string description, string configuration,
		                       string copyright, Version version)
		{
			Title         = title;
			Product       = product;
			Company       = company;
			Description   = description;
			Configuration = configuration;
			Copyright     = copyright;
			Version       = version;
		}

		public string Title { get; }

		public string Product { get; }

		public string Company { get; }

		public string Description { get; }

		public string Configuration { get; }

		public string Copyright { get; }

		public Version Version { get; }
	}

	sealed class AssemblyTitle : Attribute<AssemblyTitleAttribute, string>
	{
		public static IMetadata<string> Default { get; } = new AssemblyTitle();

		AssemblyTitle() : base(x => x.Title) {}
	}

	sealed class AssemblyProduct : Attribute<AssemblyProductAttribute, string>
	{
		public static IMetadata<string> Default { get; } = new AssemblyProduct();

		AssemblyProduct() : base(x => x.Product) {}
	}

	sealed class AssemblyCompany : Attribute<AssemblyCompanyAttribute, string>
	{
		public static IMetadata<string> Default { get; } = new AssemblyCompany();

		AssemblyCompany() : base(x => x.Company) {}
	}

	sealed class AssemblyDescription : Attribute<AssemblyDescriptionAttribute, string>
	{
		public static IMetadata<string> Default { get; } = new AssemblyDescription();

		AssemblyDescription() : base(x => x.Description) {}
	}

	sealed class AssemblyConfiguration : Attribute<AssemblyConfigurationAttribute, string>
	{
		public static IMetadata<string> Default { get; } = new AssemblyConfiguration();

		AssemblyConfiguration() : base(x => x.Configuration) {}
	}

	sealed class AssemblyCopyright : Attribute<AssemblyCopyrightAttribute, string>
	{
		public static IMetadata<string> Default { get; } = new AssemblyCopyright();

		AssemblyCopyright() : base(x => x.Copyright) {}
	}

	sealed class AssemblyVersion : Attribute<AssemblyFileVersionAttribute, Version>
	{
		public static IMetadata<Version> Default { get; } = new AssemblyVersion();

		AssemblyVersion() : base(x => Version.Parse(x.Version)) {}
	}
}