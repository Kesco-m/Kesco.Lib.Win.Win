using System.Linq;
using System.Text.RegularExpressions;

namespace Kesco.Lib.Win
{
	/// <summary>
	/// ������ �������� ������� ��� �������� � ������ � ������ �������� ��������
	/// </summary>
	public class Replacer
	{
		public static string ReplaceRusLat(string source)
        {
            if (string.IsNullOrEmpty(source))
                return string.Empty;

			source = Regex.Replace(source, "[������������]{1}", SymbolReplace, RegexOptions.IgnoreCase);
			return source;
		}

		public static string ReplaceKeysSymbols(string source)
        {
            if (string.IsNullOrEmpty(source))
                return string.Empty;

			return ReplaceExtSymbols(source.Replace("'","''").
				Replace("%","[%]").
				Replace("_","[_]").
				Replace( (char)9, ' '));
		}

		public static string ReplaceSQLSymbols(string source)
		{
			return string.IsNullOrEmpty(source) ? string.Empty : Regex.Replace(source, "[" + Regex.Escape(@"]\*+?|{[()^$.#%'") + "]+", "", RegexOptions.IgnoreCase);
		}

		public static string ReplaceExtSymbols(string source)
		{
		    return string.IsNullOrEmpty(source) ? string.Empty : Regex.Replace(source, "[" + Regex.Escape("\t\r\n-'\"([{,;!?:.*") + "]+", " ", RegexOptions.IgnoreCase);
		}

	    private static string SymbolReplace(Match m)
		{
			switch(m.Groups[0].Value.ToUpper())
			{
				case "�":
					return "e";
				case "�":
					return "k";
				case "�":
					return "e";
				case "�":
					return "h";
				case "�":
					return "x";
				case "�":
					return "b";
				case "�":
					return "a";
				case "�":
					return "p";
				case "�":
					return "o";
				case "�":
					return "c";
				case "�":
					return "m";
				case "�":
					return "t";
				default:
					return m.Groups[0].Value;
			}
		}

		/// <summary>
		/// ������� ��� ������������ �������
		/// </summary>
        /// <param name="source">�������� ������������� ������</param>
		/// <returns>������ ��� ������������ �������� � �������� � ������ � �����</returns>
        public static string ReplaceNonPrintableCharacters(string source)
		{
		    return string.IsNullOrEmpty(source) ? string.Empty : new string(source.Trim().Where(c => !char.IsControl(c)).ToArray()).Trim();
		}

	    /// <summary>
		/// �������� ��������� ������� ��� SQL
		/// </summary>
		/// <param name="s">�������� ������������� ������</param>
		/// <returns>������ �������������� ��������</returns>
		public static string ReplaceLike(string s)
        {
            if (string.IsNullOrEmpty(s))
                return string.Empty;

			s = s.Replace("'", "''");
			s = s.Replace("[", "[[]");
			s = s.Replace("_", "[_]");
			s = s.Replace("%", "[%]");
			s = s.Replace((char)9, ' ');
			s = s.Replace((char)13, ' ');
			s = s.Replace((char)10, ' ');
			s = s.Trim();

			while (s.IndexOf("  ") != -1)
				s = s.Replace("  ", " ");

			return s;
		}
	}
}