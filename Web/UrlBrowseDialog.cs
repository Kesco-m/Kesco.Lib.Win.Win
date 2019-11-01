using System;
using System.Windows.Forms;

namespace Kesco.Lib.Win.Web
{
	public class UrlBrowseDialog : FreeDialog
	{
        private string Url;
        private CookieCollection cc;
        private ExtendedBrowserControl webBrowser;
		private System.ComponentModel.IContainer components = null;

		public UrlBrowseDialog(string URL, string labelText):base()
		{
			// This call is required by the Windows Form Designer.
			this.DoubleBuffered = true; InitializeComponent();
			this.Text = labelText;
            Url = URL;
			this.webBrowser.WBWantsToClose += new EventHandler(webBrowser_HandleDestroyed);
			// TODO: Add any initialization after the InitializeComponent call
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UrlBrowseDialog));
            this.webBrowser = new ExtendedBrowserControl();
            this.SuspendLayout();
            // 
            // webBrowser
            // 
            resources.ApplyResources(this.webBrowser, "webBrowser");
            this.webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser.Name = "webBrowser";
            // 
            // UrlBrowseDialog
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.webBrowser);
            this.Name = "UrlBrowseDialog";
            this.Load += new System.EventHandler(this.UrlBrowseDialog_Load);
            this.ResumeLayout(false);

		}
		#endregion

		#region Accessers

		public CookieCollection Collection
		{
			get { return cc;}
		}

		#endregion

		private void UrlBrowseDialog_Load(object sender, System.EventArgs e)
		{
			if (!string.IsNullOrEmpty(Url) && Url != "about:blank")
			{
				this.webBrowser.SelfNavigate = true;
				this.webBrowser.Url = new Uri(Url);
			}
		}

		private void webBrowser_HandleDestroyed(object sender, EventArgs e)
		{
			this.webBrowser.WBWantsToClose -= new System.EventHandler(webBrowser_HandleDestroyed);

			if (webBrowser != null && webBrowser.Document != null)
			{
                string cookie = webBrowser.Document.Cookie;
                cc = new CookieCollection(cookie);

				Cookie dlgRez = cc.GetCookie("DlgRez");

				if ((dlgRez != null) && (dlgRez.Value != null) && (dlgRez.Value != "0"))
				{
					this.DialogResult = DialogResult.OK;
				}
			}
			End( this.DialogResult);
		}
	}
}

