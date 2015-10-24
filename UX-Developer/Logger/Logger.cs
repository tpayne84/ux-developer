using System;
using System.Collections.Generic;

namespace UX_Developer.Logger
{
	/// <summary>
	/// Generic Logging Class
	/// </summary>
	public class Logger
	{
		/// <summary>
		/// Default Constructor - Init Lists
		/// </summary>
		public Logger()
		{
			Entries = new List<string>();
			DebugEntries = new List<string>();
			Verbose = true;
			_indent = "";
		}

		/// <summary>
		/// Toggles Console.WriteLine
		/// </summary>
		public bool Verbose { get; set; }

		/// <summary>
		/// List of Log Entries
		/// </summary>
		public List<string> Entries { get; set; }

		/// <summary>
		/// List of Log Entries
		/// </summary>
		public List<string> DebugEntries { get; set; }

		/// <summary>
		/// Aggregated Log Entries
		/// </summary>
		public string Report
		{
			get { return string.Join( "\n", Entries ); }
		}

		/// <summary>
		/// Aggregated Log Entries
		/// </summary>
		public string DebugReport
		{
			get { return string.Join( "\n", DebugEntries ); }
		}

		/// <summary>
		/// Private Indentation Level string.
		/// </summary>
		private string _indent { get; set; }

		/// <summary>
		/// Increases Indentation Level
		/// </summary>
		public void ResetIndentation()
		{
			_indent = "";
		}
	
		/// <summary>
		/// Increases Indentation Level
		/// </summary>
		public void Indent()
		{
			_indent += '\t';
		}

		/// <summary>
		/// Increases Indentation Level
		/// </summary>
		public void NewLine()
		{
			WriteLine();
		}

		/// <summary>
		/// Decreases Indentation Level
		/// </summary>
		public void Unindent()
		{
			_indent = _indent.TrimEnd( '\t' );
		}

		/// <summary>
		/// Logs the Entry to the Report and DebugReport... As well as Prints the Message
		/// </summary>
		/// <param name="message"> </param>
		/// <param name="increaseIndentation"> </param>
		public void Log( string message = "", bool increaseIndentation = false )
		{
			if( increaseIndentation )
				_indent += '\t';

			if( Verbose )
				Console.WriteLine( _indent + message );

			DebugEntries.Add( _indent + message );
			Entries.Add( _indent + message );

			if( increaseIndentation )
				_indent = _indent.TrimEnd( '\t' );
		}

		/// <summary>
		/// Logs the Entry to the Report and DebugReport... As well as Prints the Message .. All Without Indentation
		/// </summary>
		/// <param name="message"> </param>
		public void WriteLine(string message = "" )
		{
			message = message.Trim();
			if (Verbose)
				Console.WriteLine(message);

			DebugEntries.Add(message);
			Entries.Add(message);
		}

		/// <summary>
		/// Adds the Entry to the Report, but does not Print the Message.
		/// </summary>
		/// <param name="message"> </param>
		/// <param name="increaseIndentation"> </param>
		public void AppendToReport( string message, bool increaseIndentation = false )
		{
			if( increaseIndentation )
				_indent += '\t';

			Entries.Add( _indent + message );

			if( increaseIndentation )
				_indent = _indent.TrimEnd( '\t' );
		}

		/// <summary>
		/// Adds the Entry to the DebugReport, Prints the Message.
		/// </summary>
		/// <param name="message"> </param>
		/// <param name="increaseIndentation"> </param>
		public void Debug( string message, bool increaseIndentation = false )
		{
			if( increaseIndentation )
				_indent += '\t';

			DebugEntries.Add( _indent + message );

			if (Verbose)
				System.Diagnostics.Debug.WriteLine( _indent + message );

			if( increaseIndentation )
				_indent = _indent.TrimEnd( '\t' );
		}
	}
}