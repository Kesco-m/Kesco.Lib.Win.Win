using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Kesco.Lib.Win.Trees;

namespace Kesco.Lib.Win.EmployeeDialog
{
	public class EmployeeSelect : UserControl
	{
		private const int userGroupIndex = 0;
		private const int houseIndex = 1;

		private bool oldPlaces;

		private bool locked;

		private bool placesLocked;
		private bool freeLocked;

		private int unitID;
		private int companyID;
		private int placeID;

		private string connectionString;
		private string connectionString2;

		private int SearchingEmpID;

		public event EventHandler SelectedEmployeeChanged;

		private GroupBox groupUnits;
		private DTree tree;
		private ImageList imageList;
		private Splitter splitter1;
		private Panel panel1;
		private CheckBox checkInsiders;
		private PictureBox photo;
		private GroupBox groupEmployees;
		private ListView list;
		private CheckBox checkPlaces;
		private CheckBox checkGuests;
		private CheckBox checkFree;
		private Panel panel2;
		private Panel panel3;
		private EmpSearchBlock empSearchBlock1;
		private PhoneList phoneList1;
		private IContainer components;

		public EmployeeSelect()
		{
			Status = EmpStatus.Resting;
			InitializeComponent();
			oldPlaces = Places;

			checkInsiders.Enabled = Places;
			checkFree.Enabled = Places;
			checkGuests.Enabled = !Places;
		}

		public EmployeeSelect(string connectionString)
		{
			Status = EmpStatus.Resting;
			this.connectionString = connectionString;
			InitializeComponent();
			oldPlaces = Places;

			checkInsiders.Enabled = Places;
			checkFree.Enabled = Places;
			checkGuests.Enabled = !Places;
		}

		#region Accessors

		public bool Insiders
		{
			get { return checkInsiders.Checked; }
			set { checkInsiders.Checked = value; }
		}

		public bool Places
		{
			get { return checkPlaces.Checked; }
			set { checkPlaces.Checked = value; }
		}

		public bool Free
		{
			get { return checkFree.Checked; }
			set { checkFree.Checked = value; }
		}

		public bool Guests
		{
			get { return checkGuests.Checked; }
			set { checkGuests.Checked = value; }
		}

		public string ExternServer { get; set; }

		public EmpStatus Status { get; set; }

		public string ConnectionString
		{
			get { return connectionString; }
			set
			{
				connectionString = value;
				if(tree != null)
					tree.ConnectionString = value;
				if(empSearchBlock1 != null)
					empSearchBlock1.ConnectionString = connectionString;
			}
		}

		public string ConnectionString2
		{
			get { return connectionString2; }
			set
			{
				connectionString2 = value;
				if(empSearchBlock1 != null)
					empSearchBlock1.ConnectionString = connectionString;
			}
		}

		public ListView List
		{
			get { return list; }
		}

		public Trees.DTree Tree
		{
			get { return tree; }
		}

		public bool MultiSelect
		{
			get { return list.MultiSelect; }
			set { list.MultiSelect = value; }
		}

		public bool PlacesLocked
		{
			get { return placesLocked; }
			set
			{
				placesLocked = value;
				checkPlaces.Enabled = !placesLocked;
			}
		}

		public bool FreeLocked
		{
			get { return freeLocked; }
			set
			{
				freeLocked = value;
				checkFree.Enabled = !freeLocked;
			}
		}

		public Options.Folder OptionFolder { get; set; }

		public int PlaceID
		{
			get { return placeID; }
			set
			{
				placeID = value;
				empSearchBlock1.PlaceID = placeID;
			}
		}

		#endregion

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

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EmployeeSelect));
			this.groupUnits = new System.Windows.Forms.GroupBox();
			this.tree = new Trees.DTree();
			this.imageList = new System.Windows.Forms.ImageList(this.components);
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.panel1 = new System.Windows.Forms.Panel();
			this.panel3 = new System.Windows.Forms.Panel();
			this.phoneList1 = new PhoneList();
			this.groupEmployees = new System.Windows.Forms.GroupBox();
			this.list = new System.Windows.Forms.ListView();
			this.photo = new System.Windows.Forms.PictureBox();
			this.checkFree = new System.Windows.Forms.CheckBox();
			this.checkPlaces = new System.Windows.Forms.CheckBox();
			this.checkGuests = new System.Windows.Forms.CheckBox();
			this.checkInsiders = new System.Windows.Forms.CheckBox();
			this.panel2 = new System.Windows.Forms.Panel();
			this.empSearchBlock1 = new EmpSearchBlock();
			this.groupUnits.SuspendLayout();
			this.panel1.SuspendLayout();
			this.panel3.SuspendLayout();
			this.groupEmployees.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.photo)).BeginInit();
			this.panel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupUnits
			// 
			this.groupUnits.Controls.Add(this.tree);
			resources.ApplyResources(this.groupUnits, "groupUnits");
			this.groupUnits.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupUnits.Name = "groupUnits";
			this.groupUnits.TabStop = false;
			// 
			// tree
			// 
			this.tree.AllowAdd = false;
			this.tree.AllowDelete = false;
			this.tree.AllowEdit = false;
			this.tree.AllowFind = false;
			this.tree.AllowMove = false;
			this.tree.ColorInsert = System.Drawing.Color.SkyBlue;
			this.tree.ColorMove = System.Drawing.Color.CornflowerBlue;
			this.tree.ConnectionString = null;
			resources.ApplyResources(this.tree, "tree");
			this.tree.FullLoad = true;
			this.tree.HideSelection = false;
			this.tree.IDField = null;
			this.tree.ImageList = this.imageList;
			this.tree.ItemHeight = 16;
			this.tree.Name = "tree";
			this.tree.NodeType = typeof(DTreeNode);
			this.tree.SelectedNode = null;
			this.tree.TableName = null;
			this.tree.TextField = null;
			this.tree.FillNode += new FillNodeEventHandler(this.tree_FillNode);
			this.tree.RefreshNodeEvent += new RefreshNodeDelegate(this.tree_RefreshNodeEvent);
			this.tree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tree_AfterSelect);
			// 
			// imageList
			// 
			this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
			this.imageList.TransparentColor = System.Drawing.SystemColors.Control;
			this.imageList.Images.SetKeyName(0, "");
			this.imageList.Images.SetKeyName(1, "");
			this.imageList.Images.SetKeyName(2, "");
			// 
			// splitter1
			// 
			resources.ApplyResources(this.splitter1, "splitter1");
			this.splitter1.Name = "splitter1";
			this.splitter1.TabStop = false;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.panel3);
			this.panel1.Controls.Add(this.panel2);
			resources.ApplyResources(this.panel1, "panel1");
			this.panel1.Name = "panel1";
			// 
			// panel3
			// 
			this.panel3.Controls.Add(this.phoneList1);
			this.panel3.Controls.Add(this.groupEmployees);
			this.panel3.Controls.Add(this.photo);
			this.panel3.Controls.Add(this.checkFree);
			this.panel3.Controls.Add(this.checkPlaces);
			this.panel3.Controls.Add(this.checkGuests);
			this.panel3.Controls.Add(this.checkInsiders);
			resources.ApplyResources(this.panel3, "panel3");
			this.panel3.Name = "panel3";
			// 
			// phoneList1
			// 
			resources.ApplyResources(this.phoneList1, "phoneList1");
			this.phoneList1.Name = "phoneList1";
			// 
			// groupEmployees
			// 
			resources.ApplyResources(this.groupEmployees, "groupEmployees");
			this.groupEmployees.Controls.Add(this.list);
			this.groupEmployees.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupEmployees.Name = "groupEmployees";
			this.groupEmployees.TabStop = false;
			// 
			// list
			// 
			resources.ApplyResources(this.list, "list");
			this.list.FullRowSelect = true;
			this.list.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.list.HideSelection = false;
			this.list.Name = "list";
			this.list.ShowItemToolTips = true;
			this.list.UseCompatibleStateImageBehavior = false;
			this.list.View = System.Windows.Forms.View.Details;
			this.list.SelectedIndexChanged += new System.EventHandler(this.list_SelectedIndexChanged);
			// 
			// photo
			// 
			resources.ApplyResources(this.photo, "photo");
			this.photo.Name = "photo";
			this.photo.TabStop = false;
			// 
			// checkFree
			// 
			resources.ApplyResources(this.checkFree, "checkFree");
			this.checkFree.Name = "checkFree";
			this.checkFree.CheckedChanged += new System.EventHandler(this.checkFree_CheckedChanged);
			this.checkFree.VisibleChanged += new System.EventHandler(this.checkFree_VisibleChanged);
			// 
			// checkPlaces
			// 
			resources.ApplyResources(this.checkPlaces, "checkPlaces");
			this.checkPlaces.Name = "checkPlaces";
			this.checkPlaces.CheckedChanged += new System.EventHandler(this.checkPlaces_CheckedChanged);
			// 
			// checkGuests
			// 
			resources.ApplyResources(this.checkGuests, "checkGuests");
			this.checkGuests.Name = "checkGuests";
			this.checkGuests.CheckedChanged += new System.EventHandler(this.checkGuests_CheckedChanged);
			this.checkGuests.VisibleChanged += new System.EventHandler(this.checkGuests_VisibleChanged);
			// 
			// checkInsiders
			// 
			resources.ApplyResources(this.checkInsiders, "checkInsiders");
			this.checkInsiders.Name = "checkInsiders";
			this.checkInsiders.CheckedChanged += new System.EventHandler(this.checkInsiders_CheckedChanged);
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.empSearchBlock1);
			resources.ApplyResources(this.panel2, "panel2");
			this.panel2.Name = "panel2";
			// 
			// empSearchBlock1
			// 
			resources.ApplyResources(this.empSearchBlock1, "empSearchBlock1");
			this.empSearchBlock1.BackColor = System.Drawing.SystemColors.Control;
			this.empSearchBlock1.EmpText = "";
			this.empSearchBlock1.FindEmployeeID = 0;
			this.empSearchBlock1.LabelText = "Найти сотрудника :";
			this.empSearchBlock1.Name = "empSearchBlock1";
			this.empSearchBlock1.PlaceID = 0;
			this.empSearchBlock1.Status = 0;
			this.empSearchBlock1.FindEmployeeEvent += new FindEmployee(this.empSearchBlock1_FindEmployeeEvent);
			// 
			// EmployeeSelect
			// 
			this.Controls.Add(this.groupUnits);
			this.Controls.Add(this.splitter1);
			this.Controls.Add(this.panel1);
			this.DoubleBuffered = true;
			this.Name = "EmployeeSelect";
			resources.ApplyResources(this, "$this");
			this.Load += new System.EventHandler(this.EmployeeSelect_Load);
			this.SizeChanged += new System.EventHandler(this.EmployeeSelect_SizeChanged);
			this.groupUnits.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.panel3.ResumeLayout(false);
			this.groupEmployees.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.photo)).EndInit();
			this.panel2.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		#region Fill

		public void FillTree()
		{
			SaveID();

			if(!Places)
			{
				tree.BeginUpdate();
				tree.Nodes.Clear();
				tree.TableName = "Инвентаризация.dbo.ЛицаЗаказчики";
				tree.IDField = "КодЛица";
				tree.TextField = "КраткоеНазваниеРус";

				DTreeNode rootNode = new DTreeNode(0, "Все");
				tree.Nodes.Add(rootNode);

				using(DataTable dt = GetCompanies(placeID, false))
				{
					foreach(DataRow dr in dt.Rows)
					{
						var node = new DTreeNode(
							(int)dr["КодЛица"],
							(dr["Лицо"].ToString() != "") ? dr["Лицо"].ToString() : dr["Кличка"].ToString());
						rootNode.Nodes.Add(node);
					}
					dt.Dispose();
				}

				rootNode.ExpandAll();
				tree.EndUpdate();
			}
			else
			{
				const string querySelect =
					"SELECT " +
					"Должности.КодДолжности," +
					"Должности.Должность," +
					"Должности.Подразделение," +
					"ISNULL(Лица.Лицо,'Организация '+Convert(varchar,Должности.КодЛица)) организация," +
					"ISNULL(U2.Сотрудник,'[вакантно]') сотрудник," +
					"Должности.Совместитель," +
					"Должности.Parent," +
					"Должности.L," +
					"Должности.R," +
					"ISNULL(U1.ФИО,'[неизвестно]') AS Изменил," +
					"Должности.Изменено " +
					"FROM Инвентаризация.dbo.vwДолжности Должности";

				string queryJoin =
					"LEFT OUTER JOIN Инвентаризация.dbo.Сотрудники U1 ON Должности.Изменил = U1.КодСотрудника " +
					"LEFT OUTER JOIN Инвентаризация.dbo.Сотрудники U2 ON Должности.КодСотрудника = U2.КодСотрудника " +
					"INNER JOIN " + ExternServer + ".Справочники.dbo.vwЛицаХолдинга Лица ON Должности.КодЛица = Лица.КодЛица";

				const string queryWhere = "WHERE Должности.Подразделение <> ''";

				const string queryOrderBy = "";//"ORDER BY L";

				tree.SetQueryString(querySelect, queryJoin, queryWhere, queryOrderBy);

				tree.TableName = "Инвентаризация.dbo.vwДолжности";
				tree.IDField = "КодДолжности";
				tree.TextField = "Подразделение";

				tree.Fill();
			}

			if(tree.Nodes.Count > 0)
				tree.SelectedNode = (DTreeNode)tree.Nodes[0];

			LoadID();
		}

		public void FillList(bool isSearch)
		{
			list.BeginUpdate();
			list.Items.Clear();
			int searchID = 0;

			DTreeNode node = tree.SelectedNode;
			DataTable dt = null;

			if(node != null)
			{
				InitColumns();

				if(!Places)
				{
					if(!isSearch)
						dt = GetEmployees(node.ID);
					else
					{
						int unitID = 0;
						string employeeName = "";

						const string selectString =
							"SELECT TOP 1 КодСотрудника, Сотрудник, КодЛицаЗаказчика" +
							" FROM Инвентаризация.dbo.Сотрудники" +
							" WHERE Состояние <= @Status" +
							" AND КодСотрудника = @EmpID";

						using(var cmd = new SqlCommand(selectString))
						{
							cmd.Connection = new SqlConnection(connectionString);
							cmd.Connection.Open();

							cmd.Parameters.Add(new SqlParameter("@Status", SqlDbType.Int) { Value = Status });
							cmd.Parameters.Add(new SqlParameter("@EmpID", SqlDbType.Int) { Value = SearchingEmpID });

							try
							{
								using(SqlDataReader dr = cmd.ExecuteReader())
								{
									if(dr.Read())
									{
										unitID = (int)dr["КодЛицаЗаказчика"];
										employeeName = dr["Сотрудник"].ToString();
									}
								}
							}
							catch { }
							finally
							{
								cmd.Connection.Close();
							}
						}
						if(unitID > 0)
						{
							tree.AfterSelect -= tree_AfterSelect;
							SelectNode(unitID, tree.Nodes);
							tree.Select();
							dt = GetEmployees(unitID);
							tree.AfterSelect += tree_AfterSelect;
						}
					}

					if(dt != null)
						for(int i = 0; i < dt.Rows.Count; i++)
						{
							var li = new PlaceListItem(dt.Rows[i]["Сотрудник"].ToString());
							li.EmployeeID = (int)dt.Rows[i]["КодСотрудника"];
							li.ToolTipText = li.Text;
							if(isSearch)
								if(li.EmployeeID == SearchingEmpID)
									searchID = i;
							list.Items.Add(li);
						}
				}
				else
				{
					if(tree.SelectedNode != null)
					{
						int id = tree.SelectedNode.ID;
						AddListItem(GetPlace(id), true);

						dt = GetSubPlaces(id);

						for(int i = 0; i < dt.Rows.Count; i++)
							AddListItem(dt.Rows[i], false);
					}
				}
			}

			list.EndUpdate();

			if(list.Items.Count > 0)
				list.Items[searchID].Selected = true;
			else
			{
				photo.Image = null;
				if(SelectedEmployeeChanged != null)
					SelectedEmployeeChanged(this, new EventArgs());
			}
		}

		private bool SelectNode(int id, TreeNodeCollection tnColl)
		{
			foreach(DTreeNode tn in tnColl)
			{
				if((tn).ID == id)
				{
					if(tree.SelectedNode != tn)
						tree.SelectedNode = tn;
					return true;
				}
				if(SelectNode(id, tn.Nodes))
					return true;
			}
			return false;
		}

		private void AddListItem(DataRow dr, bool boss)
		{
			if(dr == null)
				return;

			PlaceListItem li;
			bool mainJob = false;
			var param = new string[2];

			if((dr["КодСотрудника"] != DBNull.Value))
			{
				param[0] = dr["Сотрудник"].ToString();
				param[1] = dr["Должность"].ToString();
				li = new PlaceListItem(param) { EmployeeID = (int)dr["КодСотрудника"] };

				var cowork = (byte)dr["Совместитель"];
				mainJob = (cowork == 0);
			}
			else
			{
				if(Free)
				{
					param[0] = "[Вакантно]";
					param[1] = dr["Должность"].ToString();
					li = new PlaceListItem(param);
				}
				else
					return;
			}

			li.PlaceID = (int)dr["КодДолжности"];

			if(boss)
				li.ForeColor = Color.Blue;
			else
			{
				if(!mainJob)
					li.ForeColor = Color.DarkGray;
			}

			list.Items.Add(li);
		}

		private int removedNodeID;

		private void RefreshTreeNode()
		{
			if(!tree.SelectedNode.Equals(tree.TopNode))
			{
				tree.BeginUpdate();
				int nodeIndex = tree.SelectedNode.Index;
				removedNodeID = tree.SelectedNode.ID;
				tree.TopNode.Nodes.Remove(tree.SelectedNode);
				DataTable dt = GetCompanies(true);
				if(dt != null)
				{
					var node = new DTreeNode(
						(int)dt.Rows[0]["КодЛица"],
						(dt.Rows[0]["Лицо"].ToString() != "") ? dt.Rows[0]["Лицо"].ToString() : dt.Rows[0]["Кличка"].ToString());
					tree.TopNode.Nodes.Insert(nodeIndex, node);
					tree.SelectedNode = node;
				}
				tree.SelectedNode.ExpandAll();
				tree.EndUpdate();
				FillList(false);
			}
			else
				FillTree();
		}

		#endregion

		#region Get Data

		public DataTable GetCompanies(bool isRefresh)
		{
			if(connectionString == null && connectionString2 == null)
				return null;
			string where = "";
			if(isRefresh)
				where = " WHERE КодЛица = @EmpCode";
			return GetDataTable("SELECT DISTINCT  КодЛица, Кличка, КраткоеНазваниеРус Лицо" +
						" FROM Инвентаризация.dbo.ЛицаЗаказчики " +
						where +
						" ORDER BY КраткоеНазваниеРус",
						(isRefresh) ? new SqlParameter[] { new SqlParameter("@EmpCode", SqlDbType.Int) { Value = removedNodeID } } : null,
				 "Ошибка GetCompanies");
		}

		public DataTable GetCompanies(int placeID, bool isRefresh)
		{
			if(connectionString == null && connectionString2 == null)
				return null;

			if(placeID <= 0 || isRefresh)
				return GetCompanies(isRefresh);

			return GetDataTable("SELECT DISTINCT dbo.ЛицаЗаказчики.КодЛица, Кличка, КраткоеНазваниеРус Лицо\n" +
							" FROM dbo.Сотрудники\nINNER JOIN dbo.РабочиеМеста ON dbo.Сотрудники.КодСотрудника = dbo.РабочиеМеста.КодСотрудника\nINNER JOIN" +
							" dbo.vwРасположения Place ON dbo.РабочиеМеста.КодРасположения = Place.КодРасположения\nINNER JOIN" +
							" dbo.vwРасположения Parent ON Place.L >= Parent.L AND Place.R <= Parent.R\nINNER JOIN" +
							" dbo.ЛицаЗаказчики ON dbo.Сотрудники.КодЛицаЗаказчика = dbo.ЛицаЗаказчики.КодЛица" +
							" WHERE Parent.КодРасположения = @PlaceID",
							new SqlParameter[] { new SqlParameter("@PlaceID", SqlDbType.Int) { Value = placeID } },
							"Ошибка GetCompanies");
		}

		private DataTable GetEmployees(int personID)
		{
			if(connectionString == null)
				return null;

			string where = " WHERE emp.Состояние <= @Status", join = "";
			string selectString = "SELECT DISTINCT emp.КодСотрудника, emp.Сотрудник" +
						" FROM Инвентаризация.dbo.Сотрудники emp\n";

			if(personID > 0)
				where += " AND emp.КодЛицаЗаказчика = @PersonID";

			if(placeID > 0)
			{
				join = " INNER JOIN Инвентаризация.dbo.РабочиеМеста ON emp.КодСотрудника = dbo.РабочиеМеста.КодСотрудника" +
							" INNER JOIN Инвентаризация.dbo.vwРасположения Place ON dbo.РабочиеМеста.КодРасположения = Place.КодРасположения" +
						" INNER JOIN Инвентаризация.dbo.vwРасположения Parent ON Place.L >= Parent.L AND Place.R <= Parent.R ";
				where += " AND Parent.КодРасположения = @PlaceID";
			}

			selectString += join + where + " ORDER BY emp.Сотрудник";

			List<SqlParameter> pars = new List<SqlParameter>();
			if(personID > 0)
				pars.Add(new SqlParameter("@PersonID", SqlDbType.Int) { Value = personID });

			if(placeID > 0)
				pars.Add(new SqlParameter("@PlaceID", SqlDbType.Int) { Value = placeID });

			pars.Add(new SqlParameter("@Status", SqlDbType.Int) { Value = Status });

			return GetDataTable(selectString, pars.ToArray(), "Ошибка GetEmployees");
		}

		private DataTable GetSubPlaces(int placeID)
		{
			string selectString;

			if(Insiders)
				selectString =
					"SELECT Children.КодДолжности, Children.Должность, Children.КодСотрудника, Сотрудники.Сотрудник, Children.Совместитель" +
					" FROM Инвентаризация.dbo.vwДолжности Children INNER JOIN" +
					" Инвентаризация.dbo.vwДолжности Parent ON Children.L > Parent.L AND Children.R < Parent.R LEFT OUTER JOIN" +
					" Инвентаризация.dbo.Сотрудники Сотрудники ON Children.КодСотрудника = Сотрудники.КодСотрудника" +
					" WHERE Parent.КодДолжности = @ParentID" +
					" ORDER BY Сотрудники.Сотрудник";
			else
				selectString =
					"SELECT Должности.КодДолжности, Должности.Должность, Должности.КодСотрудника, Сотрудники.Сотрудник, Должности.Совместитель" +
					" FROM Инвентаризация.dbo.vwДолжности Должности LEFT OUTER JOIN" +
					" Инвентаризация.dbo.Сотрудники Сотрудники ON Должности.КодСотрудника = Сотрудники.КодСотрудника" +
					" WHERE Должности.Parent = @ParentID AND Должности.Подразделение = ''" +
					" ORDER BY Сотрудники.Сотрудник";


			return GetDataTable(selectString,
				new SqlParameter[] { new SqlParameter("@ParentID", SqlDbType.Int) { Value = placeID } },
				"Ошибка GetSubPlaces");
		}

		private DataRow GetPlace(int placeID)
		{
			DataTable dt = GetDataTable("SELECT Должности.КодДолжности, Должности.Должность, Должности.КодСотрудника, Сотрудники.Сотрудник, Должности.Совместитель" +
					" FROM Инвентаризация.dbo.vwДолжности Должности LEFT OUTER JOIN Инвентаризация.dbo.Сотрудники Сотрудники" +
					" ON Должности.КодСотрудника = Сотрудники.КодСотрудника" +
					" WHERE Должности.КодДолжности = @PlaceID",
					new SqlParameter[] { new SqlParameter("@PlaceID", SqlDbType.Int) { Value = placeID } },
					"Ошибка GetPlace");
			return (dt != null && dt.Rows.Count > 0) ? dt.Rows[0] : null;
		}

		internal protected DataTable GetDataTable(string query, SqlParameter[] addParams, string caption)
		{
			using(var conn = new SqlConnection(connectionString))
			using(var cmd = new SqlCommand(query, conn))
			{
				if(addParams != null)
					cmd.Parameters.AddRange(addParams);
				try
				{
					cmd.Connection.Open();
					DataTable dt = new DataTable();
					using(SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
					{
						if(dr.HasRows)
						{
							dt.Load(dr);
							dr.Close();
						}
						return dt;
					}
				}
				catch(Exception ex)
				{
					MessageBox.Show(ex.Message, caption);
					return null;
				}
			}
		}

		public Image GetPhoto(int empID)
		{
			Image img = null;
			using(var cmd = new SqlCommand())
			{
				cmd.CommandText = "SELECT TOP 1 Фотография, DATALENGTH(Фотография) AS Размер FROM Инвентаризация.dbo.ФотографииСотрудников Where КодСотрудника = " + empID.ToString() + " ORDER BY Изменено DESC, ДатаФотографирования DESC";
				cmd.Connection = new SqlConnection(connectionString);
				cmd.Connection.Open();

				try
				{
					using(SqlDataReader dr = cmd.ExecuteReader())
					{
						if(dr.Read())
						{
							var size = (int)dr["Размер"];

							if(size > 0)
							{
								var buf = new byte[size];
								dr.GetBytes(dr.GetOrdinal("Фотография"), 0, buf, 0, size);

								if(buf != null)
								{
									var stream = new MemoryStream(buf);
									try
									{
										img = Image.FromStream(stream);
									}
									catch //(ArgumentException ex)
									{
										// bad photo...
									}
								}
							}
						}
					}
				}
				catch(SqlException sqlEx)
				{
					MessageBox.Show(sqlEx.Message, "Ошибка доступа к базе данных GetPhoto");
				}
				finally
				{
					cmd.Connection.Close();
				}
			}

			return img;
		}

		private string[] GetPhones(int empID)
		{
			if(connectionString == null)
				return null;

			const string selectString = @"SELECT ТелефонныйНомер FROM dbo.vwТелефонныеНомера ТН
				  INNER JOIN dbo.ТипыТелефонныхНомеров ТТН ON ТТН.КодТипаТелефонныхНомеров = ТН.КодТипаТелефонныхНомеров
				  WHERE КодСотрудника = @EmpID
				  ORDER BY ТТН.sort";

			DataTable dt = GetDataTable(
					selectString,
				  new SqlParameter[] { new SqlParameter("@EmpID", SqlDbType.Int) { Value = empID } }, "Ошибка GetPhones");

			if(dt != null && dt.Rows.Count > 0)
			{
				var res = new string[dt.Rows.Count];
				for(int i = 0; i < dt.Rows.Count; i++)
					res[i] = dt.Rows[i]["ТелефонныйНомер"].ToString();
				return res;
			}
			return null;
		}

		#endregion

		#region Checks

		private void checkInsiders_CheckedChanged(object sender, EventArgs e)
		{
			FillList(false);
		}

		private void checkPlaces_CheckedChanged(object sender, EventArgs e)
		{
			checkInsiders.Enabled = Places;
			checkFree.Enabled = Places && !freeLocked;
			checkGuests.Enabled = !Places;

			list.Columns.Clear();
			InitColumns();

			if(!Places || !string.IsNullOrEmpty(ExternServer))
				FillTree();

			oldPlaces = Places;
		}

		private void checkGuests_CheckedChanged(object sender, EventArgs e)
		{
			Status = checkGuests.Checked ? EmpStatus.Guest : EmpStatus.Resting;

			FillList(false);
		}

		private void checkFree_CheckedChanged(object sender, EventArgs e)
		{
			FillList(false);
		}

		#endregion

		#region Select

		private void tree_AfterSelect(object sender, TreeViewEventArgs e)
		{
			phoneList1.Visible = false;
			empSearchBlock1.ClearText();
			FillList(false);
		}

		private void list_SelectedIndexChanged(object sender, EventArgs e)
		{
			int count = list.SelectedItems.Count;

			// reading photo

			if(count > 0)
			{
				if(count == 1)
				{
					var li = (PlaceListItem)list.SelectedItems[0];
					Image img = GetPhoto(li.EmployeeID);

					if(img != null)
					{
						int wid = photo.Width;
						int hei = photo.Height;

						if(img.Height * photo.Width < img.Width * photo.Height)
						{
							hei = img.Height * photo.Width / img.Width;
							wid = photo.Width;
						}
						else
						{
							wid = img.Width * photo.Height / img.Height;
							hei = photo.Height;
						}

						photo.Image = new Bitmap(img, wid, hei);
					}
					else
						photo.Image = null;

					phoneList1.AddPhones(GetPhones(li.EmployeeID));
				}
				else
					photo.Image = null;

				photo.Update();
			}

			if(SelectedEmployeeChanged != null)
				SelectedEmployeeChanged(this, e);
		}

		#endregion

		#region State

		public void LoadState(Options.Folder optFolder)
		{
			if(optFolder == null)
				return;

			OptionFolder = optFolder;

			string boolObj;
			bool tempPlaces = Places;

			if(!locked)
			{
				boolObj = optFolder.LoadStringOption("Places", tempPlaces.ToString());
				tempPlaces = Convert.ToBoolean(boolObj);
			}

			if(!locked || tempPlaces)
			{
				boolObj = optFolder.LoadStringOption("Free", Free.ToString());
				Free = Convert.ToBoolean(boolObj);

				boolObj = optFolder.LoadStringOption("Insiders", Insiders.ToString());
				Insiders = Convert.ToBoolean(boolObj);

				unitID = optFolder.LoadIntOption("UnitID", 0);
			}

			if(!locked || !tempPlaces)
			{
				boolObj = optFolder.LoadStringOption("Guests", Guests.ToString());
				Guests = Convert.ToBoolean(boolObj);

				companyID = optFolder.LoadIntOption("CompanyID", 0);
			}

			checkInsiders.Enabled = tempPlaces;
			checkFree.Enabled = tempPlaces && !freeLocked;
			checkGuests.Enabled = !tempPlaces;

			Places = tempPlaces;
		}

		public void SaveState()
		{
			if(OptionFolder == null)
				return;

			if(!locked)
				OptionFolder.Option("Places").Value = Places.ToString();

			if(!locked || Places)
			{
				OptionFolder.Option("Free").Value = Free.ToString();
				OptionFolder.Option("Insiders").Value = Insiders.ToString();
			}

			if(!locked || !Places)
				OptionFolder.Option("Guests").Value = Guests.ToString();

			SaveID();

			OptionFolder.Save();
		}

		private void LoadID()
		{
			if(OptionFolder == null)
				return;

			int id;
			if(Places)
			{
				id = unitID;
				if(id > 0)
					tree.SelectNode(id);
			}
			else
			{
				id = companyID;
				var rootNode = (DTreeNode)tree.Nodes[0];

				if(id > 0)
				{
					for(int i = 0; i < rootNode.Nodes.Count; i++)
					{
						var node = (DTreeNode)rootNode.Nodes[i];
						if(node.ID == id)
						{
							tree.SelectedNode = node;
							break;
						}
					}
				}
			}
		}

		private void SaveID()
		{
			if(OptionFolder == null)
				return;
			DTreeNode node = tree.SelectedNode;
			if(node == null)
				return;

			int id = node.ID;
			if(oldPlaces)
			{
				unitID = id;
				OptionFolder.OptionForced<int>("UnitID").Value = id;
			}
			else
			{
				companyID = id;
				OptionFolder.OptionForced<int>("CompanyID").Value = id;
			}
		}

		#endregion

		private void EmployeeSelect_Load(object sender, EventArgs e)
		{
			if(this.DesignMode)
				return;
			tree.ConnectionString = connectionString;
			if(!Places || !string.IsNullOrEmpty(ExternServer))
				FillTree();
		}

		private void InitColumns()
		{
			if(list.Columns.Count > 0)
				return;

			bool rus = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName.Equals("ru");
			if(!Places)
			{
				list.Columns.Add(rus ? "Сотрудник" : "Employee", list.Width - SystemInformation.VerticalScrollBarWidth - 2, HorizontalAlignment.Left);
				list.HeaderStyle = ColumnHeaderStyle.None;
			}
			else
			{
				list.Columns.Add(rus ? "Сотрудник" : "Employee", 200, HorizontalAlignment.Left);
				list.Columns.Add("Должность", list.Width - 202 - SystemInformation.VerticalScrollBarWidth, HorizontalAlignment.Left);
				list.HeaderStyle = ColumnHeaderStyle.Nonclickable;
			}
		}

		public void ShowFindDialog()
		{
			new EmployeeFindDialog(this).ShowDialog(FindForm());
		}

		private void tree_FillNode(object sender, FillNodeEventArgs e)
		{
			bool topLevel = (e.row["Parent"] == null);

			e.nd.Text = string.Concat((topLevel ? "(" + (string)e.row["Организация"] + ") " : ""), e.row["Подразделение"]);

			if(!topLevel && Places)
			{
				e.nd.ImageIndex = userGroupIndex;
				e.nd.SelectedImageIndex = userGroupIndex;
			}
			else
			{
				e.nd.ImageIndex = houseIndex;
				e.nd.SelectedImageIndex = houseIndex;
			}
		}

		private void EmployeeSelect_SizeChanged(object sender, EventArgs e)
		{
            Console.WriteLine("{0}: EmployeeSelect Size Changed", DateTime.Now.ToString("HH:mm:ss fff"));
			if(Width <= 0 || Width >= panel1.Width)
				return;
            Console.WriteLine("{0}: EmployeeSelect too small", DateTime.Now.ToString("HH:mm:ss fff"));
			panel1.Width = Width > 100 ? Width - 50 : Width / 2;
		}

		private void tree_RefreshNodeEvent()
		{
			tree.TextField = "EmpSelect";
			empSearchBlock1.ClearText();
			RefreshTreeNode();
		}

		private void empSearchBlock1_FindEmployeeEvent(int empID)
		{
			if(empID <= 0)
				return;
			SearchingEmpID = empID;
			FillList(true);
			empSearchBlock1.Focus();
		}

		private void checkFree_VisibleChanged(object sender, EventArgs e)
		{
			SetPhoneListPosition();
		}

		private void checkGuests_VisibleChanged(object sender, EventArgs e)
		{
			SetPhoneListPosition();
		}

		private void SetPhoneListPosition()
		{
			if(checkFree.Visible && checkGuests.Visible)
				phoneList1.Top = checkGuests.Location.Y + checkGuests.Height;
			if(!checkFree.Visible && checkGuests.Visible)
				phoneList1.Top = checkGuests.Location.Y + checkGuests.Height;
			if(checkFree.Visible && !checkGuests.Visible)
				phoneList1.Top = checkFree.Location.Y + checkFree.Height;
			if(!checkFree.Visible && !checkGuests.Visible)
				phoneList1.Top = checkInsiders.Location.Y + checkInsiders.Height;
			phoneList1.Height = photo.Height + photo.Top - phoneList1.Top;
		}
	}

	public enum EmpStatus
	{
		Working,
		Resting,
		Guest,
		Fired,
		Renter,
		FiredRenter
	}
}