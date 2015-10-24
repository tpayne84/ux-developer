using System.IO;
using System.Xml.Serialization;
using UX_Developer.Logger;

namespace UX_Developer
{
	public static class VersionHelper
	{
		/// <summary>
		/// Reset Version all Version Grouping Version to Zero.
		/// </summary>
		public static void ResetVersions( Logger.Logger logger, application appdef )
		{
			logger.NewLine();
			int groups = appdef.version.Split('.').Length;
			logger.Log( string.Format( "- Reseting All {0} Version Groups", groups ) );

			using( new Indentation( logger ) )
			{
				logger.NewLine();
				appdef.version = "0";
				for( var g = 0; g < groups - 1; g++ )
					appdef.version += ".0";

				logger.Log("- Version = " + appdef.version);
			}
		}

		/// <summary>
		/// Increments a Version Group.
		/// </summary>
		/// <param name="appdef">Deserialized appdef</param>
		/// <param name="logger"> Engine Object </param>
		/// <param name="groupIndex"> Specify Version Group to Increment... Defaults to the Last Group </param>
		public static void IncrementVersion( application appdef, Logger.Logger logger, int groupIndex = -1 )
		{
			logger.NewLine();
			logger.Log( "- Incrementing Version..." );

			string [] versionGroups = appdef.version.Split('.');

			using( new Indentation( logger ) )
			{
				if( groupIndex == -1 )
					groupIndex = versionGroups.Length -1;

				logger.Log("- From: " + appdef.version);
				versionGroups[groupIndex] = (int.Parse(versionGroups[groupIndex]) + 1).ToString();

				appdef.version = string.Join(".", versionGroups);
				logger.Log("- To: " + appdef.version);
			}
		}

		/// <summary>
		/// Updates the Version in the AppDef.xml / Also Updates the ServerCore Extension Object's Assembly Path if needed.
		/// </summary>
		/// <param name="logger">Logger</param>
		/// <param name="appdef">Deserialized AppDef</param>
		/// <param name="appdefpath">Path to AppDef</param>
		public static void UpdateAppDef(Logger.Logger logger, application appdef, string appdefpath)
		{
			using( logger.CaptionBlock("Updating AppDef Version") )
			{
				IncrementVersion(appdef, logger);

				// IDisposable StreamWriter, for saving the AppDef.xml
				using (StreamWriter sw = new StreamWriter(appdefpath))
				{
					XmlSerializer serializer = new XmlSerializer(typeof(application));

					serializer.Serialize(sw, appdef);
				}
			}
		}
	}
}