using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Kesco.Lib.Win.EmployeeDialog
{
	public class EmpSearchBlock : UserControl
	{
		private int findEmployeeID;
		private int placeID;
		private string connectionString = string.Empty;

		/// <summary>
		/// состояние поиска
		/// </summary>
		private int status;
		private SynchronizedCollection<Keys> keyLocker;
		private Keys lastKeyData = Keys.None;
		private string lastEmployeeText;
		private EmployeeParser parser;
		private UpDownTextBox textEmp;
		private System.Windows.Forms.ComboBox comboBoxEmp;
		private System.Windows.Forms.Label label1;
		private System.ComponentModel.Container components = null;
		public event FindEmployee FindEmployeeEvent;

		public EmpSearchBlock()
		{
			InitializeComponent();
			keyLocker = new SynchronizedCollection<Keys>();
		}

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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EmpSearchBlock));
			this.comboBoxEmp = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.textEmp = new UpDownTextBox();
			this.SuspendLayout();
			// 
			// comboBoxEmp
			// 
			resources.ApplyResources(this.comboBoxEmp, "comboBoxEmp");
			this.comboBoxEmp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxEmp.Name = "comboBoxEmp";
			this.comboBoxEmp.DropDown += new System.EventHandler(this.comboBoxEmp_DropDown);
			this.comboBoxEmp.SelectionChangeCommitted += new System.EventHandler(this.comboBoxEmp_SelectionChangeCommitted);
			this.comboBoxEmp.KeyUp += new System.Windows.Forms.KeyEventHandler(this.comboBoxEmp_KeyUp);
			this.comboBoxEmp.Leave += new System.EventHandler(this.comboBoxEmp_Leave);
			// 
			// label1
			// 
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			// 
			// textEmp
			// 
			resources.ApplyResources(this.textEmp, "textEmp");
			this.textEmp.HideSelection = false;
			this.textEmp.Name = "textEmp";
			this.textEmp.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textEmp_KeyDown);
			this.textEmp.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textEmp_KeyUp);
			this.textEmp.Leave += new System.EventHandler(this.textEmp_Leave);
			// 
			// EmpSearchBlock
			// 
			this.BackColor = System.Drawing.SystemColors.Control;
			this.Controls.Add(this.label1);
			this.Controls.Add(this.textEmp);
			this.Controls.Add(this.comboBoxEmp);
			this.DoubleBuffered = true;
			this.Name = "EmpSearchBlock";
			resources.ApplyResources(this, "$this");
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		#region Accessors

		public string ConnectionString
		{
			set
			{
				connectionString = value;
				if(!string.IsNullOrEmpty(connectionString))
					parser = new EmployeeParser(connectionString, placeID, status);
			}
		}

		public int PlaceID
		{
			get { return placeID; }
			set
			{
				placeID = value;
				if(!string.IsNullOrEmpty(connectionString))
					parser = new EmployeeParser(connectionString, placeID, status);
			}
		}

		public int FindEmployeeID
		{
			get { return findEmployeeID; }
			set { findEmployeeID = value; }
		}

		public int Status
		{
			get { return status; }
			set
			{
				status = value;
				if(!string.IsNullOrEmpty(connectionString))
					if(parser == null)
						parser = new EmployeeParser(connectionString, placeID, status);
					else
						parser.Load(connectionString, placeID, status);
			}
		}

		public string EmpText
		{
			get { return textEmp.Text.Trim(); }
			set { textEmp.Text = value; }
		}

		public string LabelText
		{
			get { return label1.Text; }
			set
			{
				label1.Text = value;
				comboBoxEmp.Left = textEmp.Left = label1.Right + 4;
				comboBoxEmp.Width = textEmp.Width = this.Width - label1.Right - 8;
			}
		}

		#endregion

		public void OnFindEmployee(int empID)
		{
			if(FindEmployeeEvent != null)
				FindEmployeeEvent(empID);
		}

		private void textEmp_KeyUp(object sender, KeyEventArgs e)
		{
			if(keyLocker.Contains(e.KeyData))
				return;

			keyLocker.Add(e.KeyData);
			try
			{
				string txt = textEmp.Text.Trim();
				lastKeyData = e.KeyData;
				if(lastEmployeeText != textEmp.Text &&
					!(textEmp.Text.Length == 1 && Regex.IsMatch(textEmp.Text, "[A-Za-z]")))
				{
					if(e.KeyData == Keys.Enter)
					{
						if(findEmployeeID > 0)
							OnFindEmployee(findEmployeeID);
						else
						{
							parser.Parse(ref txt);
							if(txt.Length > 0 && parser.CandidateCount > 0)
								OnFindEmployee((int)parser.CandidateEmployees[0]["КодСотрудника"]);
						}
					}
					if((lastKeyData != Keys.Delete) && (lastKeyData != Keys.Back) && e.KeyData != Keys.Down &&
						e.KeyData != Keys.Up && e.KeyData != Keys.Space)
						ParseEmployee(false);
					else if(lastKeyData == Keys.Delete || lastKeyData == Keys.Back)
					{
						findEmployeeID = 0;
						if(comboBoxEmp.DroppedDown)
							comboBoxEmp.DroppedDown = false;
						if(textEmp.Text.Trim().Length == 0)
							OnFindEmployee(0);
					}
				}
				lastEmployeeText = textEmp.Text;
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			finally
			{
				keyLocker.Remove(e.KeyData);
			}
		}

		private void ParseEmployee(bool skip)
		{
			string txt = textEmp.Text.Trim();
			string regTxt = txt;
			DataRow[] emps = parser.Parse(ref txt);
			if(emps.Length == 1)
			{
				if(textEmp.Text != txt)
				{
					textEmp.Text = txt;
					regTxt = "^(?<text>" + regTxt + ").*$";

					Match m = Regex.Match(txt, regTxt.Replace("-", " "), RegexOptions.IgnoreCase);
					if(m.Success)
					{
						int len = m.Groups["text"].Value.Length;
						textEmp.Select(len, textEmp.Text.Length - len);
					}
				}
				if(findEmployeeID != (int)emps[0]["КодСотрудника"])
				{
					findEmployeeID = (int)emps[0]["КодСотрудника"];
					OnFindEmployee(findEmployeeID);
				}
			}
			else
			{
				if((emps.Length != 1) || (!textEmp.Text.Equals(emps[0]["Сотрудник"])))
				{
					findEmployeeID = 0;
					OnFindEmployee(findEmployeeID);
				}
				for(int i = 0; i < emps.Length; i++)
					textEmp.Text =
						textEmp.Text.Replace(emps[i]["ФИО"].ToString(), "").Trim(new char[] { ' ', ',', ';' });
			}

			if(!skip)
				comboBoxEmp.DroppedDown = false;
			if(parser.CandidateCount > 1 && parser.CandidateCount < 9)
			{
				ShowEmployees(parser.CandidateEmployees);
				var findForm = FindForm();
				if(findForm != null)
					findForm.Cursor = Cursors.Default;
			}
		}

		private void ShowEmployees(List<DataRow> drs)
		{
			comboBoxEmp.Items.Clear();
			Form findForm = comboBoxEmp.FindForm();
			if(findForm != null)
				findForm.Cursor = Cursors.Default;
			for(int i = 0; i < drs.Count; i++)
			{
				ListItem item = new ListItem((int)drs[i]["КодСотрудника"], new string[2] { drs[i]["Сотрудник"].ToString(), drs[i]["Сотрудник"].ToString() });
				if(!DBNull.Value.Equals(drs[i]["Дополнение"]) && !string.IsNullOrEmpty(drs[i]["Дополнение"] as string))
					if(i > 0 && drs[i]["Сотрудник"].Equals(drs[i - 1]["Сотрудник"]))
						item.Text += " (" + drs[i]["Дополнение"].ToString() + ")";
					else if(i < drs.Count - 1 && drs[i]["Сотрудник"].Equals(drs[i + 1]["Сотрудник"]))
						item.Text += " (" + drs[i]["Дополнение"].ToString() + ")";
				comboBoxEmp.Items.Add(item);
			}
			comboBoxEmp.DropDownWidth = DropDownWidth(comboBoxEmp);
			comboBoxEmp.DroppedDown = true;
			if(findForm != null)
				findForm.Cursor = Cursors.Default;
		}

		private void textEmp_Leave(object sender, System.EventArgs e)
		{
			comboBoxEmp.DroppedDown = false;
		}

		private void comboBoxEmp_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if(e.KeyData == Keys.Enter)
			{
				textEmp.Text = ((ListItem)comboBoxEmp.SelectedItem).SubItems[1].Text;
				int id = ((ListItem)comboBoxEmp.SelectedItem).ID;
				comboBoxEmp.DroppedDown = false;
				textEmp.Select();
				OnFindEmployee(id);
			}
		}

		private int DropDownWidth(ComboBox myCombo)
		{
			int maxWidth = myCombo.Width, temp = 0;
			foreach(var obj in myCombo.Items)
			{
				temp = TextRenderer.MeasureText(myCombo.GetItemText(obj), myCombo.Font).Width;
				if(temp > maxWidth)
				{
					maxWidth = temp;
				}
			}
			return maxWidth;
		}

		private void comboBoxEmp_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			if(comboBoxEmp.SelectedItem != null)
			{
				lastEmployeeText = ((ListItem)comboBoxEmp.SelectedItem).SubItems[1].Text;
				textEmp.Text = lastEmployeeText;
				findEmployeeID = ((ListItem)comboBoxEmp.SelectedItem).ID;
				textEmp.Select();
				UpdateData();
			}
		}

		private void comboBoxEmp_Leave(object sender, System.EventArgs e)
		{
			comboBoxEmp.DroppedDown = false;
		}

		public void ClearText()
		{
			textEmp.Clear();
			lastEmployeeText = "";
			findEmployeeID = 0;
			OnFindEmployee(findEmployeeID);
		}

		private void textEmp_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			switch(e.KeyData)
			{
				case Keys.Up:
					if(comboBoxEmp.DroppedDown)
					{
						textEmp.Leave -= new EventHandler(textEmp_Leave);
						comboBoxEmp.Select();
						comboBoxEmp.SelectedItem = comboBoxEmp.Items[comboBoxEmp.Items.Count - 1];
						comboBoxEmp.FindForm().Cursor = Cursors.Default;
						textEmp.Leave += new EventHandler(textEmp_Leave);
					}
					break;
				case Keys.Down:
					if(comboBoxEmp.DroppedDown)
					{
						textEmp.Leave -= new EventHandler(textEmp_Leave);
						comboBoxEmp.Select();
						comboBoxEmp.SelectedItem = comboBoxEmp.Items[0];
						comboBoxEmp.FindForm().Cursor = Cursors.Default;
						textEmp.Leave += new EventHandler(textEmp_Leave);
					}
					break;
			}
		}

		private void comboBoxEmp_DropDown(object sender, EventArgs e)
		{
			Cursor.Current = Cursors.Default;
		}

		public void SelectEmployee(int employeeID)
		{
			parser.Load(connectionString, placeID, status);
			findEmployeeID = employeeID;
		}

		public void UpdateData()
		{
			if(this.DesignMode)
				return;
			if(findEmployeeID > 0)
			{
				DataRow dr = parser.SetEmployee(findEmployeeID);
				if(dr != null)
				{
					lastEmployeeText = dr["Сотрудник"].ToString();
					textEmp.Text = lastEmployeeText;
					OnFindEmployee(findEmployeeID);
				}
			}
			else
				ParseEmployee(true);
		}

		public void ReloadData()
		{
			if(parser != null)
				parser.Load(connectionString, placeID, status);
		}

		public List<DataRow> GetCandidateEmployees()
		{
			if(textEmp.Text.Trim().Length == 0)
				return null;
			if(parser != null)
				return parser.CandidateEmployees;
			return null;
		}
	}

	public delegate void FindEmployee(int empID);
}