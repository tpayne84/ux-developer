using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Threading;
using MFilesAPI;
using UX_Developer.Logger;
using UX_Developer.resources;

namespace UX_Developer
{
	internal class Program
	{
		/// <summary>
		/// Console Logger with several helper functions
		/// </summary>
		private static Logger.Logger Logger { get; set; }

		/// <summary>
		/// Settings for this configuration
		/// </summary>
		private static UXDevSettings Settings { get; set; }

		/// <summary>
		/// Console Application Entry Point
		/// </summary>
		/// <param name="args">Drag the Settings file onto the executable in order to use it as a parameter</param>
		private static void Main( string [] args )
		{
			// Initialize the logger.
			Logger = new Logger.Logger();

			// Create a Caption Block to print the settings
			using( Logger.CaptionBlock("Reading Settings") )
			{
				// populate the Settings object.
				Settings = new UXDevSettings(args, Logger);
			}

			// increment the appdef version.
			VersionHelper.UpdateAppDef(Logger, Settings.AppDef, Settings.AppDefPath);

			// Package the UX-App Directory.
			string packedApp = UXPack( Settings.DirectoryToZip, Settings.Outputname );

			// Install the application to the vault.
			InstallApp( packedApp, Settings );		
		}

		/// <summary>
		/// Installer Method - Install app to Vault.
		/// </summary>
		/// <param name="packedApp">mfappx Path</param>
		/// <param name="settings">Populated UXDevSettings Object</param>
		private static void InstallApp( string packedApp, UXDevSettings settings)
		{
			// Create a caption block for the installation.
			using( Logger.CaptionBlock( "Connecting to Vault" ) )
			{
				// Connect to Vault
				var serverApp = new MFilesServerApplication();
				
				// report the creation
				Logger.WriteLine("Server App Created => Connecting Administratively");

				// Connect Administrative => As the Take Vault on/off-line requires it.
				serverApp.ConnectAdministrative(
					null,
					settings.AuthType,
					settings.Username,
					settings.Password,
					settings.Domain,
					settings.ProtocolSequence,
					settings.ServerAddress,
					settings.Port,
					settings.LocalComputerName
					);

				// Report the successful connection.
				Logger.WriteLine(@"Connected to server.");

				// Log into the Vault
				Vault vault = serverApp.LogInToVault(settings.VaultGuid);

				// Report the successful Log On.
				Logger.WriteLine(@"Logged into Vault: " + vault.Name);

				// create a caption block to make the output more readable.
				using( Logger.CaptionBlock( "Installing Application" ) )
				{
					// Try to install the application.
					try
					{
						Logger.WriteLine(@"Attempting Application Installation");

						// Install the Application to the Vault.
						vault.CustomApplicationManagementOperations.InstallCustomApplication(packedApp);

						// If no error was thrown, report the success.
						Logger.WriteLine(@"Application Installed Successfully");
					}
					catch (Exception ex)
					{
						// A common error is the already exists.
						Logger.WriteLine( ex.Message.StartsWith( @"Already exists" )
							? ex.Message.Substring( 0, ex.Message.IndexOf( '\n' ) )
							: ex.Message );
					}
				}

				// create a caption block to make the output more readable.
				using (Logger.CaptionBlock("Post Installation Processing"))
				{
					// Determine if a Auto-Allow Installation Registry file should be created.
					if (settings.GenerateAutoInstallReg)
					{
						Logger.WriteLine("Generating the Auto Installation Registry File");

						// Get the Template Text and inject values
						string reg = StringTemplate.AllowSilentInstall;
						reg = reg.Replace("$VERSION", vault.GetServerVersionOfVault().Display);
						reg = reg.Replace("$NAME", Path.GetFileNameWithoutExtension(settings.Outputname));
						reg = reg.Replace("$VAULTGUID", vault.GetGUID());
						reg = reg.Replace("$APPGUID", settings.AppDef.guid);

						// create the registry file path.
						string regPath = Path.Combine(settings.DirectoryToZip, @"allow_auto_install.reg");

						// Delete and destroy the existing reg file... if there is one.
						if (File.Exists(regPath))
							File.Delete(regPath);

						// Write the reg file to disk.
						File.WriteAllText(regPath, reg);

						Logger.WriteLine("Registry File Created: " + regPath);
					}

					// Should the vault be restarted after processing?
					if (settings.RestartVault)
					{
						Logger.WriteLine("About to Restart the Vault");

						Logger.WriteLine("\t- Logging out.");
						
						// TODO - This does not seem to work, a manual logout is recommended.
						vault.LogOutSilent();

						// Try to take the vault on/off-line.
						try
						{
							Logger.WriteLine("\t- Taking Vault Offline");
							serverApp.VaultManagementOperations.TakeVaultOffline(settings.VaultGuid, true);

							Logger.WriteLine("\t- Bringing Vault back Online");
							serverApp.VaultManagementOperations.BringVaultOnline(settings.VaultGuid);
							Logger.WriteLine("Vault Now Online");
						}
						catch (Exception ex)
						{
							// report the error
							Logger.WriteLine(ex.Message);
						}

						// TODO - I need to find a better way to close open client windows, but until then...
						if (settings.KillExplorerWindows)
						{
							Logger.WriteLine("Closing Open Explorer Windows.");
							foreach (Process p in Process.GetProcessesByName("explorer"))
								p.Kill();
						}

						Logger.WriteLine(@"Waiting a few seconds...");
						Thread.Sleep(1000 * 2);
					}

					// Determine if we should open the Vault Window for the Developer.
					if (Settings.OpenVault)
					{
						Logger.WriteLine(@"Opening Path: " + settings.LocalVaultPath);

						// Open the path in the default application.
						Process.Start(Settings.LocalVaultPath);
					}
				}
			}

			// Should we leave the console window open or not?
			if (!settings.AutoExitApp)
			{
				Logger.NewLine();
				Logger.WriteLine(@"Press any key to exit...");
				Console.ReadKey();
			}
		}

		/// <summary>
		/// This method will zip a directory and save it using the outputName.
		/// </summary>
		/// <param name="dirToZip">Directory to Zip</param>
		/// <param name="outputName">Filename to save it as... ie UX_App.mfappx</param>
		/// <returns></returns>
		private static string UXPack( string dirToZip, string outputName )
		{
			// Get the full path of the package.
			string fullpath = Path.Combine(Directory.GetParent(dirToZip).FullName, outputName);

			using( Logger.CaptionBlock( "Packaging UX-App" ) )
			{
				Logger.WriteLine(@"Target path: " + fullpath);

				if (File.Exists(fullpath))
				{
					Logger.WriteLine(@"Deleting existing file: " + fullpath);
					File.Delete(fullpath);
				}

				Logger.WriteLine("Zipping Directory: " + dirToZip);
				ZipDirectory(dirToZip, fullpath);

				Logger.WriteLine("Packaging Complete.");
			}

			return fullpath;
		}

		/// <summary>
		/// Zips a Directory
		/// </summary>
		/// <param name="targetDir"> Directory to be Zipped </param>
		/// <param name="zipFile"> Output Filename </param>
		private static void ZipDirectory( string targetDir, string zipFile )
		{
			ZipFile.CreateFromDirectory( targetDir, zipFile, CompressionLevel.Fastest, false );
		}
	}

	public static class _
	{
		/// <summary>
		/// Simple helper extensions to retrieve a value from a Dictionary.
		/// </summary>
		/// <param name="d">The Dictionary</param>
		/// <param name="key">Key to Extract the Value from.</param>
		/// <returns></returns>
		public static string TryGetValueEx( this Dictionary<string, string> d, string key )
		{
			if( d.ContainsKey( key.ToLower() ) )
			{
				string val;
				d.TryGetValue(key.ToLower(), out val);
				return val;
			}

			return "";
		}
	}
}