using System;
using System.Drawing.Printing;
using System.Management;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms;

namespace Kesco.Lib.Win.Web
{
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[Docking(DockingBehavior.AutoDock)]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
    public class ExtendedBrowserControl : WebBrowser
	{
		private Uri internalUri;
		private UnsafeNativeMethods.IWebBrowser2 axIWebBrowser2;
		private string defPrinter;

        private System.Threading.Timer timer;

		const int WM_PARENTNOTIFY = 0x210;
		const int WM_DESTROY = 2;
		public event EventHandler WBWantsToClose;

		private void OnWBWantsToClose()
		{
			if(WBWantsToClose != null)
				WBWantsToClose(this, EventArgs.Empty);
		}

		public ExtendedBrowserControl()
		{
			base.DocumentCompleted += ExtendedBrowserControl_DocumentCompleted;
		}

		protected override void Dispose(bool disposing)
		{
			if(disposing)
			{
				base.DocumentCompleted -= ExtendedBrowserControl_DocumentCompleted;
			}
			base.Dispose(disposing);
		}

		private const uint NoHistory = 0x2;
		void ExtendedBrowserControl_Navigating(object sender, System.Windows.Forms.WebBrowserNavigatingEventArgs e)
		{
			if(e.Url != null && !String.IsNullOrEmpty(e.Url.AbsoluteUri) && e.Url.AbsoluteUri.StartsWith("http") && !e.Url.AbsoluteUri.Contains("#") && String.IsNullOrEmpty(e.TargetFrameName) && EnableInternalReloader && !SelfNavigate)
			{
                Console.WriteLine("{0}: +)", DateTime.Now.ToString("HH:mm:ss fff"));
			}
		}

        protected override void WndProc(ref System.Windows.Forms.Message m)
		{
			if(m != null && m.Msg == WM_PARENTNOTIFY)
			{
				if(m.WParam.ToInt32() == WM_DESTROY)
					OnWBWantsToClose();
			}

			base.WndProc(ref m);
		}

		protected override void AttachInterfaces(object nativeActiveXObject)
		{
			axIWebBrowser2 = (UnsafeNativeMethods.IWebBrowser2)nativeActiveXObject;
			base.AttachInterfaces(nativeActiveXObject);
		}

		protected override void DetachInterfaces()
		{
			axIWebBrowser2 = null;
			base.DetachInterfaces();
		}

		public void ForceNavigate(string url)
		{
			object dummy = null;
			object obj = (int)NoHistory;
			axIWebBrowser2.Navigate(url, ref obj, ref dummy, ref dummy, ref dummy);
		}

		public void Print(string printer, string printTemplate)
		{
			object input = printTemplate;
			PrinterSettings ps = new PrinterSettings();
			bool test = ps.IsDefaultPrinter;
			defPrinter = ps.PrinterName;
			bool change = (printer != defPrinter);
			WqlEventQuery q = new WqlEventQuery("SELECT * FROM __InstanceDeletionEvent WITHIN 2 WHERE TargetInstance ISA 'Win32_PrintJob'");
			ManagementEventWatcher eventWatcher = new ManagementEventWatcher(q);
			if(change)
			{
				Console.WriteLine("{0}: Start printer change to {1}", DateTime.Now.ToString("HH:mm:ss fff"), printer);
				SetDefaultPrinter(printer);
				eventWatcher.EventArrived += eventWatcher_EventArrived;
				Console.WriteLine("{0}: Printer changed to {1}", DateTime.Now.ToString("HH:mm:ss fff"), printer);
			}
			if(axIWebBrowser2.QueryStatusWB(NativeMethods.OLECMDID.OLECMDID_PRINT) == (NativeMethods.OLECMDF.OLECMDF_SUPPORTED | NativeMethods.OLECMDF.OLECMDF_ENABLED))
				axIWebBrowser2.ExecWB(NativeMethods.OLECMDID.OLECMDID_PRINT, NativeMethods.OLECMDEXECOPT.OLECMDEXECOPT_DONTPROMPTUSER, ref input, IntPtr.Zero);

			if(change)
			{
                timer = new System.Threading.Timer(new System.Threading.TimerCallback(StopManagementEventWatcher), eventWatcher, 5000, -1);
				eventWatcher.Start();
			}
		}

		public bool IsCommandEnable(NativeMethods.OLECMDID command)
		{
			if(axIWebBrowser2 != null)
				return axIWebBrowser2.QueryStatusWB(command) == (NativeMethods.OLECMDF.OLECMDF_SUPPORTED | NativeMethods.OLECMDF.OLECMDF_ENABLED);
			else
				return false;
		}

		public void ExecCopy()
		{
			object input = IntPtr.Zero;
			if(axIWebBrowser2 != null && axIWebBrowser2.QueryStatusWB(NativeMethods.OLECMDID.OLECMDID_COPY) == (NativeMethods.OLECMDF.OLECMDF_SUPPORTED | NativeMethods.OLECMDF.OLECMDF_ENABLED))
			{
				axIWebBrowser2.ExecWB(NativeMethods.OLECMDID.OLECMDID_COPY, NativeMethods.OLECMDEXECOPT.OLECMDEXECOPT_DODEFAULT, ref input, IntPtr.Zero);
			}
		}

		private void StopManagementEventWatcher(object state)
		{
			lock(timer)
			{
				if(timer != null)
				{

					timer.Dispose();
					timer = null;
					ManagementEventWatcher watcher = state as ManagementEventWatcher;
					if(watcher != null)
					{
						watcher.Stop();
						watcher.Dispose();
						watcher = null;
					}
				}
			}
		}

		private void eventWatcher_EventArrived(object sender, EventArrivedEventArgs e)
		{
			StopManagementEventWatcher(sender);
			SetDefaultPrinter(defPrinter);
		}

		private void SetDefaultPrinter(string PrinterName)
		{
			try
			{
				ManagementObject managementObject = new ManagementObject("Win32_Printer.DeviceID='" + PrinterName + "'");
				ManagementBaseObject outParams;
                Console.WriteLine("{0}: Get WMI object", DateTime.Now.ToString("HH:mm:ss fff"));
				if(managementObject != null)
				{
					outParams = managementObject.InvokeMethod("SetDefaultPrinter", null, null);
					Int32 retVal = (int)(uint)outParams.Properties["ReturnValue"].Value;
					if(retVal == 0)
                        Console.WriteLine("{0}: setdefaultprinter", DateTime.Now.ToString("HH:mm:ss fff"));
					else
                        Console.WriteLine("{0}: ex!!!", DateTime.Now.ToString("HH:mm:ss fff"));
				}
				else
                    Console.WriteLine("{0}: Object is null !!!", DateTime.Now.ToString("HH:mm:ss fff"));
			}
			catch
			{
                Console.WriteLine("{0}: setdefaultprinter failed", DateTime.Now.ToString("HH:mm:ss fff"));
			}
		}

		public void NavigateTo(Uri url)
		{
#if AdvancedLogging
			try
			{
				Log.Logger.EnterMethod(this, "NavigateTo(Uri url) url = " + (url == null ? "null" : url.AbsoluteUri));
#endif
			SelfNavigate = true;

			if(Url == null || string.IsNullOrEmpty(Url.AbsoluteUri) || Url.AbsoluteUri.Equals("about:blank"))
			{
				internalUri = null;
				if(url != null && !string.IsNullOrEmpty(url.AbsoluteUri))
				{
					object dummy = null;
					object obj = (int)NoHistory;
					axIWebBrowser2.Navigate(url.AbsoluteUri, ref obj, ref dummy, ref dummy, ref dummy);
				}
			}
			else
			{
				internalUri = url;
				this.Navigate("about:blank");
			}
#if AdvancedLogging
			}
			finally
			{
				Log.Logger.LeaveMethod(this, "NavigateTo(Uri url) url= " + (url == null ? "null" : url.AbsoluteUri));
			}
#endif
		}

		public bool SelfNavigate
		{
			get;
			set;
		}

		public bool EnableInternalReloader
		{
			get;
			set;
		}

		public Uri InternalUri
		{
			get { return internalUri; }
		}

		void ExtendedBrowserControl_DocumentCompleted(object sender, System.Windows.Forms.WebBrowserDocumentCompletedEventArgs e)
		{
#if AdvancedLogging
			try
			{
				Log.Logger.EnterMethod(this, "ExtendedBrowserControl_DocumentCompleted(object sender, System.Windows.Forms.WebBrowserDocumentCompletedEventArgs e) url = " + (internalUri != null ? internalUri.AbsoluteUri : "null"));
#endif
			if(internalUri != null && !internalUri.AbsoluteUri.Equals("about:blank"))
			{
				object dummy = null;
				object obj = (int)NoHistory;
				axIWebBrowser2.Navigate(internalUri.AbsoluteUri, ref obj, ref dummy, ref dummy, ref dummy);
				internalUri = null;
			}
			else
				SelfNavigate = false;
#if AdvancedLogging
			}
			finally
			{
				Log.Logger.LeaveMethod(this, "ExtendedBrowserControl_DocumentCompleted(object sender, System.Windows.Forms.WebBrowserDocumentCompletedEventArgs e) url = " + (internalUri != null ? internalUri.AbsoluteUri : "null"));
			}
#endif
		}
	}
}