using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using MFilesAPI;

namespace UX_Developer
{
	public class UXDevSettings
	{
		/// <summary>
		/// Self Populating Constructor
		/// </summary>
		/// <param name="args">args From the Program.cs</param>
		/// <param name="logger">Logger to Log things.</param>
		/// <param name="printComments">Should the comments from the settings.ini be printed?</param>
		public UXDevSettings(string[] args, Logger.Logger logger, bool printComments = false)
		{
			// read all the settings
			string[] settingsFile =
				File.ReadAllLines(args.Length != 0
					? args[0]
				// ReSharper disable once AssignNullToNotNullAttribute
					: Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().FullName), "settings.ini"));

			// local dictionary
			Dictionary<string, string> settings = new Dictionary<string, string>();

			// print out the settings file
			foreach (string line in settingsFile)
			{
				if (!string.IsNullOrWhiteSpace(line))
				{
					if (!line.StartsWith("#"))
					{
						string[] vals = line.Split('=');

						logger.WriteLine(string.Format("\t- {0} => {1}", vals[0], vals[1]));
						settings.Add(vals[0].ToLower(), vals[1]);
					}
					else
					{
						if( printComments )
							logger.WriteLine(line);
					}
				}
			}

			// create additional spacing
			logger.NewLine();

			// Populate properties from the settings.ini
			AuthType = (MFAuthType)Enum.Parse(typeof(MFAuthType), settings.TryGetValueEx("AuthType"));
			AutoExitApp = bool.Parse(settings.TryGetValueEx("AutoExitApp"));
			DirectoryToZip = settings.TryGetValueEx( "DirectoryToZip" );
			Domain = settings.TryGetValueEx( "Domain" );
			GenerateAutoInstallReg = bool.Parse( settings.TryGetValueEx( "GenerateAutoInstallReg" ));
			KillExplorerWindows = bool.Parse( settings.TryGetValueEx( "KillExplorerWindows" ));
			LocalComputerName = settings.TryGetValueEx( "LocalComputerName" );
			LocalVaultPath = settings.TryGetValueEx( "LocalVaultPath" );
			OpenVault = bool.Parse( settings.TryGetValueEx( "OpenVault" ));
			Outputname = settings.TryGetValueEx( "Outputname" );
			Password = settings.TryGetValueEx( "Password" );
			Port = settings.TryGetValueEx( "Port" );
			ProtocolSequence = settings.TryGetValueEx( "ProtocolSequence" );
			RestartVault = bool.Parse( settings.TryGetValueEx( "RestartVault" ));
			ServerAddress = settings.TryGetValueEx( "ServerAddress" );
			Username = settings.TryGetValueEx( "Username" );
			VaultGuid = settings.TryGetValueEx( "VaultGuid" );

			// Get the path to the appdef.xml
			AppDefPath = Directory.GetFiles(DirectoryToZip, "appdef.xml", SearchOption.AllDirectories).FirstOrDefault();
			// Deserialize the AppDef
			DeserializeAppDef(AppDefPath);
		}

		/// <summary>
		/// De-serializes an appdef.xml to an Object
		/// </summary>
		/// <param name="appDefPath"></param>
		private void DeserializeAppDef(string appDefPath)
		{
		   // Create an instance of stream writer.
			using( StreamReader sr = new StreamReader( appDefPath ) )
			{
				// create a serializer
				XmlSerializer s = new XmlSerializer(typeof(application));

				// Deserialize from the StreamReader
				AppDef = (application)s.Deserialize(sr);
			}
		}

		/// <summary>
		/// AppDef for this Application
		/// </summary>
		public application AppDef { get; set; }

		/// <summary>
		/// AppDef Path for this Application
		/// </summary>
		public string AppDefPath { get; set; }

		/// <summary>
		/// Directory containing the UX-App
		/// </summary>
		[DefaultValue("")]
		public string DirectoryToZip { get; set; }

		/// <summary>
		/// Output filename of the packaged application: ie. UX_App.mfappx
		/// </summary>
		[DefaultValue("UX_App.mfappx")]
		public string Outputname { get; set; }

		/// <summary>
		/// Vault GUID with Curly Braces: ie. {8719028C-B31D-496C-BAE1-77FDD12B7A12}
		/// </summary>
		[DefaultValue("")]
		public string VaultGuid { get; set; }

		/// <summary>
		/// Server Address
		/// </summary>
		[DefaultValue("localhost")]
		public string ServerAddress { get; set; }

		/// <summary>
		/// M-Files Authorization Type
		/// </summary>
		[DefaultValue(MFAuthType.MFAuthTypeLoggedOnWindowsUser)]
		public MFAuthType AuthType { get; set; }

		/// <summary>
		/// M-Files Username
		/// </summary>
		[DefaultValue("")]
		public string Username { get; set; }

		/// <summary>
		/// M-Files Password
		/// </summary>
		[DefaultValue("")]
		public string Password { get; set; }

		/// <summary>
		/// M-Files Domain
		/// </summary>
		[DefaultValue("")]
		public string Domain { get; set; }

		/// <summary>
		/// M-Files Protocol Sequence: [ ncacn_ip_tcp || ncacn_spx || ncacn_http || ncalrpc ]
		/// </summary>
		[DefaultValue("ncacn_ip_tcp")]
		public string ProtocolSequence { get; set; }

		/// <summary>
		/// M-Files Port
		/// </summary>
		[DefaultValue("2266")]
		public string Port { get; set; }

		/// <summary>
		/// M-Files Local Computer Name
		/// </summary>
		[DefaultValue("API_Test")]
		public string LocalComputerName { get; set; }

		/// <summary>
		/// When True => Takes the vault offline and back online
		/// </summary>
		[DefaultValue(false)]
		public bool RestartVault { get; set; }

		/// <summary>
		/// When True => Kills all explorer.exe processes ( in order to close all open M-Files Clients )
		/// TODO - Find a better way to close open client windows.
		/// </summary>
		[DefaultValue(false)]
		public bool KillExplorerWindows { get; set; }

		/// <summary>
		/// When True => closes the console window after completion
		/// </summary>
		[DefaultValue(true)]
		public bool AutoExitApp { get; set; }

		/// <summary>
		/// When True => Opens the 'localvaultpath' after completion
		/// </summary>
		[DefaultValue(false)]
		public bool OpenVault { get; set; }

		/// <summary>
		/// Local Path to this Vault
		/// </summary>
		[DefaultValue("")]
		public string LocalVaultPath { get; set; }

		/// <summary>
		/// When True => Generates a mostly populated registry 
		/// file to prevent a pop-up on installation of this app
		/// </summary>
		[DefaultValue(false)]
		public bool GenerateAutoInstallReg { get; set; }	
	}
}
