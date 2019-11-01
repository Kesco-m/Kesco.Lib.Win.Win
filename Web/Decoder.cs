using System;
using System.Text.RegularExpressions;

namespace Kesco.Lib.Win.Web
{
	/// <summary>
	/// Раскодирование из юникода
	/// </summary>
	public class Decoder
	{
		private const string AllowedChars = "[0-9a-fA-F]";

		public static string Decode(string s)
		{
			string rSep = Cookie.RecordSeparator;
			string uSep = Cookie.UnitSeparator;

			if (s != null)
				s = Regex.Replace(s, "%u(" + AllowedChars + "{4})", new MatchEvaluator(UnicodeByteReplace));

			if (s != null)
				s = Regex.Replace(s, "(?!" + uSep + "|" + rSep + ")%(" + AllowedChars + "{2})", new MatchEvaluator(ASCIIByteReplace));

			// "&nbsp;" -> " "
			if (s != null)
				s = Regex.Replace(s, "%A0", " ", RegexOptions.IgnoreCase);

			return s;
		}

		#region ASCII

		private static string ASCIIByteReplace(Match m)
		{
			return ASCIIByteToString(m.Groups[1].Value);
		}

		private static string ASCIIByteToString(string b)
		{
			string ret = null;
			string pat = "^" + AllowedChars + "{2}$";
			if (Regex.IsMatch(b, pat))
			{
				byte i = Convert.ToByte(b, 16);
				ret = System.Text.Encoding.ASCII.GetString(new byte[1] {i});
			}

			return ret;
		}

		#endregion

		#region Unicode
		
		private static string UnicodeByteReplace(Match m)
		{
			return UnicodeByteToString(m.Groups[1].Value);
		}

		private static string UnicodeByteToString(string b)
		{
			string ret = null;
			string pat = "^" + AllowedChars + "{4}$";

			if (Regex.IsMatch(b, pat))
			{
				Int16 i = Convert.ToInt16(b, 16);
				ret = System.Text.Encoding.Unicode.GetString(new byte[] {(byte)i, (byte)(i >> 8)});
			}

			return ret;
		}

		#endregion
	}
}