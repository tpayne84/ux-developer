using System;

namespace UX_Developer.Logger
{
	public sealed class Indentation : IDisposable
	{
		public Indentation( Logger logger )
		{
			Logger = logger;
			Logger.Indent();
		}

		private Logger Logger { get; set; }

		public void Dispose()
		{
			Logger.Unindent();
		}
	}
}