using System;
using System.Windows.Forms;

namespace Kesco.Lib.Win.Web
{
	/// <summary>
	/// Summary description for DocsLinksCreate.
	/// </summary>
	public class DocsLinksCreateDialog : FreeDialog
	{
		private int parentDocID;
		private int childDocID;
		private int fieldID;

		private string baseURL;
		private string paramStr;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private ExtendedBrowserControl browser;
		private System.ComponentModel.Container components = null;

		/// <summary>
		///  ����� ������� �������� ����� � �������, ������������ �������
		/// </summary>
		/// <param name="url">����� ��� ��������</param>
		/// <param name="ParentDocID">�� ���������</param>
		/// <param name="ChildDocID">�� �����������</param>
		/// <param name="PlaceID">��� v.1</param>
		public DocsLinksCreateDialog(string url, int ParentDocID, int ChildDocID, int PlaceID)
		{
			this.parentDocID = ParentDocID;
			this.childDocID = ChildDocID;
			this.fieldID = PlaceID;
			this.DoubleBuffered = true; InitializeComponent();

			if(url == null || url.Trim() == "")
			{
				throw new Exception("�� ������ ������ �������");
			}
			else
				baseURL = url;

			paramStr = "parentID="+parentDocID.ToString()+"&childID="+childDocID.ToString()+((fieldID>0)?"fieldID="+fieldID.ToString():"");
			this.browser.WBWantsToClose += new EventHandler(browser_WBWantsToClose);
		}

		#region Accesssors

		public int ParentDocID
		{
			get {return parentDocID;}
		}

		public int ChildDocID
		{
			get {return childDocID;}
		}

		public int PlaceID
		{
			get {return fieldID;}
		}

		#endregion

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DocsLinksCreateDialog));
			this.browser = new ExtendedBrowserControl();
			this.SuspendLayout();
			// 
			// browser
			// 
			resources.ApplyResources(this.browser, "browser");
			this.browser.EnableInternalReloader = false;
			this.browser.Name = "browser";
			this.browser.SelfNavigate = false;
			// 
			// DocsLinksCreateDialog
			// 
			resources.ApplyResources(this, "$this");
			this.Controls.Add(this.browser);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DocsLinksCreateDialog";
			this.Load += new System.EventHandler(this.DocsLinksCreate_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void DocsLinksCreate_Load(object sender, System.EventArgs e)
		{
			this.Cursor = Cursors.WaitCursor;
			browser.SelfNavigate = true;
			browser.Navigate(baseURL + "?" + paramStr);
			this.Cursor = Cursors.Default;
		}

		private void browser_WBWantsToClose(object sender, EventArgs e)
		{
			this.browser.WBWantsToClose -= new EventHandler(browser_WBWantsToClose);
			this.Close();
		}
	}
}