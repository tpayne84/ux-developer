using System.Linq;

namespace UX_Developer.Logger
{
	public static class LogHelper
	{
		/// <summary>
		/// Self Populating CaptionBlock Constructor
		/// </summary>
		/// <param name="l">Logger or Subclass</param>
		/// <param name="caption">Header Caption</param>
		/// <param name="symbol">Character used as the Header Row Line</param>
		/// <param name="maxLength">Max Header Line Length</param>
		public static CaptionBlock CaptionBlock( this Logger l, string caption, char symbol = '#', int maxLength = 80 )
		{
			return new CaptionBlock( l, caption, symbol, maxLength );
		}

		/// <summary>
		/// Self Populating Indentation
		/// </summary>
		/// <param name="l">Logger or Subclass</param>
		public static Indentation Indentation(this Logger l )
		{
			return new Indentation(l);
		}

		/// <summary>
		/// Generates a Nice Header
		/// </summary>
		/// <param name="caption"></param>
		/// <param name="symbol"></param>
		/// <param name="maxLength"></param>
		/// <returns>Formatted Double Line String</returns>
		public static string Header(string caption, char symbol = '#', int maxLength = 80)
		{
			// Generate the Top Header
			string top = symbol.Times(maxLength);

			// get the half symbol part length
			int divisor = (maxLength - (caption.Length + 2)) / 2;
			
			// Ensure no truncation is needed
			if(divisor < 3)
			{
				while( divisor < 3 )
				{
					// Trim From the Left
					caption = caption.Substring( 1, caption.Length - 1 );

					// Re-Set the Divisor
					divisor = ( maxLength - ( caption.Length + 2 ) ) / 2;
				}

				caption = "..." + caption.Substring( 3 );
			}

			// generate the bottom half part
			string part = symbol.Times(divisor);

			// generate the bottom row
			string bottom = string.Format("{0} {1} {0}", part, caption);

			// sometimes the divisor leaves it one short
			if (bottom.Length != maxLength)
				bottom += symbol;

			// return the formatted string
			return string.Format("{0}\n{1}", top, bottom);
		}
	
		/// <summary>
		/// Generates a Nice Footer
		/// </summary>
		/// <param name="caption"></param>
		/// <param name="symbol"></param>
		/// <param name="maxLength"></param>
		/// <returns>Formatted Double Line String</returns>
		public static string Footer(string caption, char symbol = '#', int maxLength = 80)
		{
			// Generate the Top Header
			string bottom = symbol.Times(maxLength);

			// get the half symbol part length
			int divisor = (maxLength - (caption.Length + 2)) / 2;
			
			// Ensure no truncation is needed
			if (divisor < 3)
			{
				while (divisor < 3)
				{
					// Trim From the Left
					caption = caption.Substring(1, caption.Length - 1);

					// Re-Set the Divisor
					divisor = (maxLength - (caption.Length + 2)) / 2;
				}

				caption = "..." + caption.Substring(3);
			}

			// generate the bottom half part
			string part = symbol.Times(divisor);

			// generate the bottom row
			string top = string.Format("{0} {1} {0}", part, caption);

			// sometimes the divisor leaves it one short
			if (top.Length != maxLength)
				top += symbol;

			// return the formatted string
			return string.Format("{0}\n{1}", top, bottom);
		}

		/// <summary>
		/// Allows for a Ruby-Like syntax for getting a string of a certain length.
		/// </summary>
		/// <param name="c">Character to Repeat</param>
		/// <param name="n">Number of Times to Repeat</param>
		/// <returns>Repeated String</returns>
		public static string Times(this char c, int n)
		{
			return string.Concat(Enumerable.Repeat(c, n));
		}

	}
}
