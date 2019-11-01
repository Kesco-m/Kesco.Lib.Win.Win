using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Kesco.Lib.Win.Web
{
	public struct UserInfo
	{
		public int ID;
		public string Name;
	}
	/// <summary>
	/// Summary description for UserDialog.
	/// </summary>
	public class UserDialog : FreeDialog
	{
		private string baseURL;
		private string paramStr;
		private ArrayList users;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private Panel browserPanel;
		private ExtendedBrowserControl browser;
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// ¬ызов диалога поиска сотрудников с адресом из реестра
		/// </summary>
		/// <param name="paramStr">строка дополнительных параметры</param>
		//		public UserDialog(string paramStr):this( null, paramStr)
		//		{
		//			
		//		}

		/// <summary>
		/// ¬ызов диалога поиска сотрудников с адресом, передаваемым строкой
		/// </summary>
		/// <param name="url">адрес дл€ открыти€</param>
		/// <param name="paramStr">строка дополнительных параметры</param>
		public UserDialog(string url, string paramStr)
		{
			//
			// Required for Windows Form Designer support
			//
			this.DoubleBuffered = true; InitializeComponent();

			if (url == null || url == "")
				throw new Exception("Ќе задана строка запуска");
			else
				baseURL = url;

			users = new ArrayList();
			this.paramStr = paramStr;
		}

		#region Accessors

		public string BaseURL
		{
			get { return baseURL; }
			set { baseURL = value; }
		}

		public string ParamStr
		{
			get { return paramStr; }
			set { paramStr = value; }
		}

		public ArrayList Users
		{
			get { return users; }
		}

		#endregion

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserDialog));
			this.browserPanel = new System.Windows.Forms.Panel();
			this.browser = new ExtendedBrowserControl();
			this.browserPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// browserPanel
			// 
			resources.ApplyResources(this.browserPanel, "browserPanel");
			this.browserPanel.Controls.Add(this.browser);
			this.browserPanel.Name = "browserPanel";
			// 
			// browser
			// 
			resources.ApplyResources(this.browser, "browser");
			this.browser.MinimumSize = new System.Drawing.Size(20, 20);
			this.browser.Name = "browser";
			// 
			// UserDialog
			// 
			resources.ApplyResources(this, "$this");
			this.Controls.Add(this.browserPanel);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "UserDialog";
			this.Load += new System.EventHandler(this.UserDialog_Load);
			this.browserPanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void UserDialog_Load(object sender, EventArgs e)
		{
			this.Cursor = Cursors.WaitCursor;
			browser.SelfNavigate = true;
            //browser.Url = new Uri(baseURL + ((baseURL.IndexOf("?") > 0) ? "&" : "?") + paramStr);
			browser.WBWantsToClose += new EventHandler(BrowserClose);
            browser.Navigate(baseURL + ((baseURL.IndexOf("?") > 0) ? "&" : "?") + paramStr);
			this.Cursor = Cursors.Default;
		}

		private void BrowserClose(object sender, System.EventArgs e)
		{
			CookieCollection cc;
			if (browser != null && browser.Document != null)
			{
				string cookie = browser.Document.Cookie;
				cc = new CookieCollection(cookie);

				Cookie dlgRez = cc.GetCookie("DlgRez");

				if ((dlgRez != null) && (dlgRez.Value != null) && (dlgRez.Value != "0"))
				{
					Cookie retVal = cc.GetCookie("RetVal");

					if ((retVal != null) && (retVal.Value != null))
					{
						string retStr = retVal.Value;
						string rSep = Cookie.RecordSeparator;
						string uSep = Cookie.UnitSeparator;

						Regex r = new Regex("(?<id>((?!" + uSep + ").)+)" + uSep + "(?<name>((?!" + rSep + ").)+)(" + rSep + "|$)", RegexOptions.IgnoreCase);
						Match m = r.Match(retStr);

						while (m.Success)
						{
							try
							{
								UserInfo user;
								user.ID = Int32.Parse(m.Groups["id"].Value);
								user.Name = System.Web.HttpUtility.UrlDecode(m.Groups["name"].Value);

								users.Add(user);
							}
							catch { }

							m = m.NextMatch();
						}

						if (users.Count > 0)
							this.DialogResult = DialogResult.OK;
					}
				}
                browser.Document.ExecCommand("ClearAuthenticationCache", false, null);
			}
			End(this.DialogResult);
		}
	}
}
;