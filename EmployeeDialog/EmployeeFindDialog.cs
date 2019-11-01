using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using Kesco.Lib.Win.Trees;

namespace Kesco.Lib.Win.EmployeeDialog
{
	/// <summary>
	/// Summary description for FindDialog.
	/// </summary>
	internal class EmployeeFindDialog : Form
	{
		private EmployeeSelect employeeSelect;
		private System.Windows.Forms.TextBox txtFind;
		private System.Windows.Forms.Button buttonFind;
		private System.Windows.Forms.Label lblFind;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.RadioButton rbEmployee;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.RadioButton rbPlace;

		private string searchString;
		private string lastFound = "";

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public EmployeeFindDialog(EmployeeSelect employeeSelect)
		{
			InitializeComponent();

			this.employeeSelect = employeeSelect;

			if(!employeeSelect.Places)
				rbPlace.Enabled = false;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if(disposing)
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EmployeeFindDialog));
			this.lblFind = new System.Windows.Forms.Label();
			this.txtFind = new System.Windows.Forms.TextBox();
			this.buttonFind = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.rbEmployee = new System.Windows.Forms.RadioButton();
			this.rbPlace = new System.Windows.Forms.RadioButton();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// lblFind
			// 
			resources.ApplyResources(this.lblFind, "lblFind");
			this.lblFind.Name = "lblFind";
			// 
			// txtFind
			// 
			resources.ApplyResources(this.txtFind, "txtFind");
			this.txtFind.Name = "txtFind";
			this.txtFind.TextChanged += new System.EventHandler(this.txtFind_TextChanged);
			// 
			// buttonFind
			// 
			resources.ApplyResources(this.buttonFind, "buttonFind");
			this.buttonFind.Name = "buttonFind";
			this.buttonFind.Click += new System.EventHandler(this.buttonFind_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.rbEmployee);
			this.groupBox1.Controls.Add(this.rbPlace);
			resources.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			// 
			// rbEmployee
			// 
			this.rbEmployee.Checked = true;
			resources.ApplyResources(this.rbEmployee, "rbEmployee");
			this.rbEmployee.Name = "rbEmployee";
			this.rbEmployee.TabStop = true;
			this.rbEmployee.CheckedChanged += new System.EventHandler(this.rbEmployee_CheckedChanged);
			// 
			// rbPlace
			// 
			resources.ApplyResources(this.rbPlace, "rbPlace");
			this.rbPlace.Name = "rbPlace";
			this.rbPlace.CheckedChanged += new System.EventHandler(this.rbPlace_CheckedChanged);
			// 
			// buttonCancel
			// 
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			resources.ApplyResources(this.buttonCancel, "buttonCancel");
			this.buttonCancel.Name = "buttonCancel";
			// 
			// EmployeeFindDialog
			// 
			this.AcceptButton = this.buttonFind;
			resources.ApplyResources(this, "$this");
			this.CancelButton = this.buttonCancel;
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.buttonFind);
			this.Controls.Add(this.txtFind);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.lblFind);
			this.DoubleBuffered = true;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "EmployeeFindDialog";
			this.ShowInTaskbar = false;
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void buttonFind_Click(object sender, System.EventArgs e)
		{
			buttonFind.Enabled = (ListFindNext(txtFind.Text));
		}

		private bool ListFindNext(string searchString)
		{
			this.searchString = searchString;
			return ListFindNext();
		}

		private bool ListFindNext()
		{
			if(searchString.Length == 0)
				return false;

			string selectString;

			int employeeID = 0;
			int unitID = 0;
			string employeeName = "";

			if(!employeeSelect.Places)	// обычный режим - выбор сотрудников
			{
				selectString =
					"SELECT TOP 1 КодСотрудника, Сотрудник, КодЛицаЗаказчика" +
					" FROM Инвентаризация.dbo.Сотрудники" +
					" WHERE Состояние <= @Status" +
					" AND Сотрудник LIKE @SearchString" +
					" AND Сотрудник > @LastFound";

				using(SqlCommand cmd = new SqlCommand(selectString))
				using(cmd.Connection = new SqlConnection(employeeSelect.ConnectionString))
				{
					cmd.Connection.Open();

					cmd.Parameters.Add(new SqlParameter("@Status", SqlDbType.Int) { Value = employeeSelect.Status });
					cmd.Parameters.Add(new SqlParameter("@SearchString", SqlDbType.NVarChar) { Value = "%" + searchString + "%" });
					cmd.Parameters.Add(new SqlParameter("@LastFound", SqlDbType.NVarChar) { Value = lastFound });

					try
					{
						using(SqlDataReader dr = cmd.ExecuteReader())
						{
							if(dr.Read())
							{
								employeeID = (int)dr["КодСотрудника"];
								unitID = (int)dr["КодЛицаЗаказчика"];
								employeeName = dr["Сотрудник"].ToString();
							}
                            dr.Close();
						}
					}
					catch { }
                    finally
					{
					    cmd.Connection.Close();
					}
				}
			}
			else	// выбор должностей
			{
				string likeString = "";

				if(rbEmployee.Checked)
					likeString = " AND Сотрудники.Сотрудник LIKE @SearchString";

				if(rbPlace.Checked)
					likeString = " AND Должности.Должность LIKE @SearchString";

				selectString =
					"SELECT TOP 1 Должности.КодДолжности, Должности.Должность, Должности.КодСотрудника, Сотрудники.Сотрудник, Должности.Parent, Должности.Подразделение" +
					" FROM Инвентаризация.dbo.vwДолжности Должности INNER JOIN" +
					" Инвентаризация.dbo.Сотрудники Сотрудники ON Должности.КодСотрудника = Сотрудники.КодСотрудника" +
					likeString +
					" AND Сотрудники.Сотрудник > @LastFound" +
					" ORDER BY Сотрудники.Сотрудник";

				using(SqlCommand cmd = new SqlCommand(selectString))
				using(cmd.Connection = new SqlConnection(employeeSelect.ConnectionString))
				{
					cmd.Connection.Open();

					cmd.Parameters.Add(new SqlParameter("@SearchString", SqlDbType.NVarChar) { Value = "%" + searchString + "%" });
					cmd.Parameters.Add(new SqlParameter("@LastFound", SqlDbType.NVarChar) { Value = lastFound });

					try
					{
					    using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
					    {
					        if(dr.Read())
					        {
					            employeeID = (int)dr["КодСотрудника"];
					            employeeName = dr["Сотрудник"].ToString();

					            string unitName = dr["Подразделение"].ToString();

					            if(unitName != "")
					                unitID = (int)dr["КодДолжности"];
					            else
					                unitID = (int)dr["Parent"];
					        }
					        dr.Close();
					    }
					}
                    catch { }
                    finally
                    {
                        cmd.Connection.Close();
                    }
				}
			}

			if(employeeID == 0)
			{
				if(lastFound == "")
				{
					MessageBox.Show("Не найдено");
				}
				else
				{
					if(MessageBox.Show("Не найдено, начать с начала?", "", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
					{
						lastFound = "";
						return ListFindNext();
					}
				}
			}
			else	// выделение соответствующего узла дерева и элемента в списке
			{
				if(!employeeSelect.Places)	// по сотрудникам
				{
					// выделяем узел в дереве

					DTreeNode rootNode = (DTreeNode)employeeSelect.Tree.Nodes[0];
					bool found = false;

					for(int i = 0; i < rootNode.Nodes.Count; i++)
					{
						DTreeNode childNode = (DTreeNode)rootNode.Nodes[i];
						if(childNode.ID == unitID)
						{
							if(employeeSelect.Tree.SelectedNode != childNode)
								employeeSelect.Tree.SelectedNode = childNode;
							found = true;
							break;
						}
					}

					if(!found)
						employeeSelect.Tree.SelectedNode = rootNode;
				}
				else
				{
					// выделяем узел в дереве

					DTreeNode node = FindNode(unitID);
					if(node != null)
						employeeSelect.Tree.SelectedNode = node;
				}

				// выделяем сотрудника в списке

				ListView list = employeeSelect.List;

				list.BeginUpdate();

				list.SelectedItems.Clear();

				list.EndUpdate();

				for(int i = 0; i < list.Items.Count; i++)
				{
					PlaceListItem li = (PlaceListItem)list.Items[i];

					if(li.EmployeeID == employeeID)
					{
						li.Selected = true;
						li.EnsureVisible();
						lastFound = li.Text;
						return true;
					}
				}
			}

			return false;
		}

		private DTreeNode FindNode(int id)
		{
			DTree tree = employeeSelect.Tree;

			const string tableName = "Инвентаризация.dbo.vwДолжности";
			const string idField = "КодДолжности";
			const string leftField = "L";
			const string rightField = "R";

			DTreeNode nd;
			using(DataTable dt = new DataTable())
			{
				string sql = " DECLARE @L int, @R  int\n" +
					" SELECT @L=" + leftField + ",@R=" + rightField + " FROM " + tableName + " WHERE " + idField + "=" + id + "\n" +
					" SELECT " + idField + " FROM " + tableName +
					" WHERE " + leftField + "<= ISNULL(@L,-1) AND " + rightField + ">= ISNULL(@R,-1) " +
					" ORDER BY " + leftField;

				using(SqlDataAdapter da = new SqlDataAdapter(sql, employeeSelect.ConnectionString))
					da.Fill(dt);

				if(dt.Rows.Count == 0)
					return null;

				TreeNodeCollection nds = tree.Nodes;
				nd = null;
				DTreeNode pnd = null;

				for(int i = 0; i < dt.Rows.Count; i++)
				{
					nd = tree.FindNodeIn(nds, (int)dt.Rows[i][idField]);
					if(nd == null)
					{
						tree.RefreshSubNodes(pnd);
						nd = tree.FindNodeIn(nds, (int)dt.Rows[i][idField]);
						if(nd == null)
							return null;
					}
					nds = nd.Nodes;
					pnd = nd;
				}
			}
			return nd;
		}

		private void txtFind_TextChanged(object sender, System.EventArgs e)
		{
			buttonFind.Enabled = true;
		}

		private void rbEmployee_CheckedChanged(object sender, System.EventArgs e)
		{
			buttonFind.Enabled = true;
		}

		private void rbPlace_CheckedChanged(object sender, System.EventArgs e)
		{
			buttonFind.Enabled = true;
		}
	}
}