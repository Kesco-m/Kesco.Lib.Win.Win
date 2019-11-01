using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Kesco.Lib.Win.EmployeeDialog
{
	public class EmployeeDialog : Form
	{
		private EmployeeSelect employeeSelect;
		private Panel panel1;
		private Button buttonCancel;
		private Button buttonOK;
		private Button buttonFind;
		private Container components;

		public EmployeeDialog(string connectionString, string connectionString2, Options.Folder optFolder)
		{
			InitializeComponent();

			employeeSelect.ConnectionString = connectionString;
			employeeSelect.ConnectionString2 = connectionString2;
			if(optFolder != null)
				employeeSelect.LoadState(optFolder);
		}

		public EmployeeDialog(string connectionString, Options.Folder optFolder) : this(connectionString, connectionString, optFolder)
		{
		}

		#region Accessors

		public ListView List
		{
			get { return employeeSelect.List; }
		}

		public bool MultiSelect
		{
			get { return employeeSelect.MultiSelect; }
			set { employeeSelect.MultiSelect = value; }
		}

		public bool Places
		{
			get { return employeeSelect.Places; }
			set { employeeSelect.Places = value; }
		}

		public bool PlacesLocked
		{
			get { return employeeSelect.PlacesLocked; }
			set { employeeSelect.PlacesLocked = value; }
		}

		public bool FreeLocked
		{
			get { return employeeSelect.FreeLocked; }
			set { employeeSelect.FreeLocked = value; }
		}

		public string ExternServer
		{
			get { return employeeSelect.ExternServer; }
			set { employeeSelect.ExternServer = value; }
		}

		public int PlaceID
		{
			get { return employeeSelect.PlaceID; }
			set { employeeSelect.PlaceID = value; }
		}

		#endregion

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
			var resources = new System.ComponentModel.ComponentResourceManager(typeof(EmployeeDialog));
			this.employeeSelect = new EmployeeSelect();
			this.panel1 = new System.Windows.Forms.Panel();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.buttonOK = new System.Windows.Forms.Button();
			this.buttonFind = new System.Windows.Forms.Button();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// employeeSelect
			// 
			resources.ApplyResources(this.employeeSelect, "employeeSelect");
			this.employeeSelect.ConnectionString = null;
			this.employeeSelect.ConnectionString2 = null;
			this.employeeSelect.ExternServer = null;
			this.employeeSelect.Free = false;
			this.employeeSelect.FreeLocked = false;
			this.employeeSelect.Guests = false;
			this.employeeSelect.Insiders = false;
			this.employeeSelect.MultiSelect = true;
			this.employeeSelect.Name = "employeeSelect";
			this.employeeSelect.OptionFolder = null;
			this.employeeSelect.PlaceID = 0;
			this.employeeSelect.Places = false;
			this.employeeSelect.PlacesLocked = false;
			this.employeeSelect.Status = EmpStatus.Resting;
			// 
			// panel1
			// 
			resources.ApplyResources(this.panel1, "panel1");
			this.panel1.Controls.Add(this.buttonCancel);
			this.panel1.Controls.Add(this.buttonOK);
			this.panel1.Controls.Add(this.buttonFind);
			this.panel1.Name = "panel1";
			// 
			// buttonCancel
			// 
			resources.ApplyResources(this.buttonCancel, "buttonCancel");
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Name = "buttonCancel";
			// 
			// buttonOK
			// 
			resources.ApplyResources(this.buttonOK, "buttonOK");
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOK.Name = "buttonOK";
			// 
			// buttonFind
			// 
			resources.ApplyResources(this.buttonFind, "buttonFind");
			this.buttonFind.Name = "buttonFind";
			this.buttonFind.Click += new System.EventHandler(this.buttonFind_Click);
			// 
			// EmployeeDialog
			// 
			this.AcceptButton = this.buttonOK;
			resources.ApplyResources(this, "$this");
			this.CancelButton = this.buttonCancel;
			this.Controls.Add(this.employeeSelect);
			this.Controls.Add(this.panel1);
			this.DoubleBuffered = true;
			this.KeyPreview = true;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "EmployeeDialog";
			this.ShowInTaskbar = false;
			this.Closing += new System.ComponentModel.CancelEventHandler(this.EmployeeDialog_Closing);
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void EmployeeDialog_Closing(object sender, CancelEventArgs e)
		{
			employeeSelect.SaveState();
		}

		private void buttonFind_Click(object sender, EventArgs e)
		{
			employeeSelect.ShowFindDialog();
		}
	}
}