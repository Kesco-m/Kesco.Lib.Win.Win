using System.Windows.Forms;

namespace Kesco.Lib.Win.EmployeeDialog
{
	/// <summary>
	/// Тип элемента списка
	/// </summary>
	public class PlaceListItem : ListViewItem
	{
	    public PlaceListItem(string text) : base(text)
		{
			EmployeeID = 0;
			PlaceID = 0;
		}

		public PlaceListItem(string[] param) : base(param)
		{
			EmployeeID = 0;
			PlaceID = 0;
		}

		#region Accessors

	    public int EmployeeID { get; set; }

	    public int PlaceID { get; set; }

	    #endregion
	}
}