using System.Linq;
using System.Text.RegularExpressions;

namespace Kesco.Lib.Win
{
	/// <summary>
	/// Модуль содержит функции для удаления и замены в строка ненужных символов
	/// </summary>
	public class Replacer
	{
		public static string ReplaceRusLat(string source)
        {
            if (string.IsNullOrEmpty(source))
                return string.Empty;

			source = Regex.Replace(source, "[ЁКЕНХВАРОСМТ]{1}", SymbolReplace, RegexOptions.IgnoreCase);
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
				case "Ё":
					return "e";
				case "К":
					return "k";
				case "Е":
					return "e";
				case "Н":
					return "h";
				case "Х":
					return "x";
				case "В":
					return "b";
				case "А":
					return "a";
				case "Р":
					return "p";
				case "О":
					return "o";
				case "С":
					return "c";
				case "М":
					return "m";
				case "Т":
					return "t";
				default:
					return m.Groups[0].Value;
			}
		}

		/// <summary>
		/// Удаляет все непечатаемые символы
		/// </summary>
        /// <param name="source">Введённая пользователем строка</param>
		/// <returns>Строка без непечатаемых символов и пробелов в начале и конце</returns>
        public static string ReplaceNonPrintableCharacters(string source)
		{
		    return string.IsNullOrEmpty(source) ? string.Empty : new string(source.Trim().Where(c => !char.IsControl(c)).ToArray()).Trim();
		}

	    /// <summary>
		/// Заменяет служебные символы для SQL
		/// </summary>
		/// <param name="s">Введённая пользователем строка</param>
		/// <returns>Строка экранированных символов</returns>
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