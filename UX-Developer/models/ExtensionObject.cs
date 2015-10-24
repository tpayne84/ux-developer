using System.Xml.Serialization;

namespace UX_Developer
{
	public class ExtensionObject
	{
		[ XmlElement( "name" ) ]
		public string Name { get; set; }

		[ XmlElement( "assembly" ) ]
		public string Assembly { get; set; }

		[ XmlElement( "class" ) ]
		public string Class { get; set; }

		[ XmlElement( "installation-method" ) ]
		public string InstallationMethod { get; set; }

		[ XmlElement( "uninstallation-method" ) ]
		public string UninstallationMethod { get; set; }

		[ XmlElement( "initialization-method" ) ]
		public string InitializationMethod { get; set; }

		[ XmlElement( "uninitialization-method" ) ]
		public string UninitializationMethod { get; set; }

		[ XmlElement( "start-operations-method" ) ]
		public string StartOperationsMethod { get; set; }
	}
}