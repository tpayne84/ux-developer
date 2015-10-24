using System;

namespace UX_Developer.Logger
{
	public class CaptionBlock : IDisposable
	{
		public CaptionBlock( Logger logger, string caption, char symbol = '#', int maxLength = 80)
		{
			Logger = logger;
			c = caption;
			s = symbol;
			m = maxLength;
			Logger.NewLine();
			Logger.WriteLine( LogHelper.Header( "BEGIN: " + c, s, m) );
			Logger.NewLine();
			Logger.Indent();		
		}

		private Logger Logger { get; set; }
		private string c;
		private char s;
		private int m;

		public void Dispose()
		{
			Logger.Unindent();
			Logger.NewLine();
			Logger.WriteLine( LogHelper.Footer("END: " + c, s, m));
			Logger.NewLine();		
		}
	}
}
