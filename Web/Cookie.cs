namespace Kesco.Lib.Win.Web
{
	/// <summary>
	/// Класс для доступа к кукам
	/// </summary>
	public class Cookie
	{
		public static string RecordSeparator = "%1E";
		public static string UnitSeparator = "%1F";

		private string name;
		private string val;

		public Cookie(string name, string val)
		{
			this.name = name;
			this.val = val;
		}

		#region Accessors

		public string Name
		{
			get { return name; }
			set { name = value; }
		}

		public string Value
		{
			get { return val; }
			set { val = value; }
		}

		#endregion
	}
}
