using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Kesco.Lib.Win.EmployeeDialog
{
	/// <summary>
	/// Summary description for EmployeeData.
	/// </summary>
	public class EmployeeData
	{
        private readonly string connectionString;
		public DataSet DS { get; private set; }

		public EmployeeData(string connectionString)
		{
			this.connectionString = connectionString;
            DS = null;
		}

		/// ������� �����������
		public int LoadTables(int placeID, int status)
		{
			string selectString = "";
			if(placeID < 1)
			{
				selectString = "SELECT �������������, ���������, ���, �������, ���, ��������, ����������, Login, �������RL, ���RL, ��������RL, ������������ FROM ��������������.dbo.���������� WHERE ��������� <= @Status";
			}
			else
			{
				selectString = "SELECT DISTINCT emp.�������������, emp.���������, ���, �������, ���, ��������, ����������, Login, �������RL, ���RL, ��������RL, ������������ " +
					" FROM ��������������.dbo.���������� emp\n" +
					" INNER JOIN ��������������.dbo.������������ ON emp.������������� = dbo.������������.�������������" +
					" INNER JOIN ��������������.dbo.vw������������ Place ON dbo.������������.��������������� = Place.���������������" +
					" INNER JOIN ��������������.dbo.vw������������ Parent ON Place.L >= Parent.L AND Place.R <= Parent.R" +
					" WHERE Parent.��������������� = @PlaceID AND ��������� <= @Status" + ((status > 3) ? " AND ��������� <> 4" : "") +
					" ORDER BY emp.���������";
			}
			using(SqlConnection cn = new SqlConnection(connectionString))
			using(SqlDataAdapter cmd = new SqlDataAdapter(selectString, cn))
			{
				if(placeID > 0)
					cmd.SelectCommand.Parameters.AddWithValue("@PlaceID", placeID);
				cmd.SelectCommand.Parameters.AddWithValue("@Status", status);
				if(DS != null)
					DS.Dispose();

                DS = new DataSet();
				try
				{
                    cmd.Fill(DS, "����������");
				}
				catch(SqlException sqlEx)
				{
					MessageBox.Show(sqlEx.Message, "������ ������� � ���� ������");
					return -1;
				}
			}
			return 0;
		}
	}
}