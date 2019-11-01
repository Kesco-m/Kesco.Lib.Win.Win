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

		/// достаем Сотрудников
		public int LoadTables(int placeID, int status)
		{
			string selectString = "";
			if(placeID < 1)
			{
				selectString = "SELECT КодСотрудника, Сотрудник, ФИО, Фамилия, Имя, Отчество, Дополнение, Login, ФамилияRL, ИмяRL, ОтчествоRL, ДатаРождения FROM Инвентаризация.dbo.Сотрудники WHERE Состояние <= @Status";
			}
			else
			{
				selectString = "SELECT DISTINCT emp.КодСотрудника, emp.Сотрудник, ФИО, Фамилия, Имя, Отчество, Дополнение, Login, ФамилияRL, ИмяRL, ОтчествоRL, ДатаРождения " +
					" FROM Инвентаризация.dbo.Сотрудники emp\n" +
					" INNER JOIN Инвентаризация.dbo.РабочиеМеста ON emp.КодСотрудника = dbo.РабочиеМеста.КодСотрудника" +
					" INNER JOIN Инвентаризация.dbo.vwРасположения Place ON dbo.РабочиеМеста.КодРасположения = Place.КодРасположения" +
					" INNER JOIN Инвентаризация.dbo.vwРасположения Parent ON Place.L >= Parent.L AND Place.R <= Parent.R" +
					" WHERE Parent.КодРасположения = @PlaceID AND Состояние <= @Status" + ((status > 3) ? " AND Состояние <> 4" : "") +
					" ORDER BY emp.Сотрудник";
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
                    cmd.Fill(DS, "Сотрудники");
				}
				catch(SqlException sqlEx)
				{
					MessageBox.Show(sqlEx.Message, "Ошибка доступа к базе данных");
					return -1;
				}
			}
			return 0;
		}
	}
}