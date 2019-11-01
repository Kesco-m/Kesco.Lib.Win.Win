using System.Windows.Forms;

namespace Kesco.Lib.Win.EmployeeDialog
{
	public class ListItem : ListViewItem
	{
	    public ListItem(int id, string text) : base(text)
		{
			ID = id;
		}

		public ListItem(int id, string[] values) : base(values)
		{
			ID = id;
		}

		#region Accessors

	    public int ID { get; set; }

	    #endregion

		public override string ToString()
		{
			return Text;
		}
	}
}