﻿using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows.Forms;

namespace Kesco.Lib.Win.Web
{
    class UnsafeNativeMethods
    {
        private UnsafeNativeMethods()
        {
        }

        [ComImport, SuppressUnmanagedCodeSecurity, TypeLibType(TypeLibTypeFlags.FOleAutomation | (TypeLibTypeFlags.FDual | TypeLibTypeFlags.FHidden)), Guid("D30C1661-CDAF-11d0-8A3E-00C04FC9E26E")]
        public interface IWebBrowser2
        {
            [DispId(100)]
            void GoBack();
            [DispId(0x65)]
            void GoForward();
            [DispId(0x66)]
            void GoHome();
            [DispId(0x67)]
            void GoSearch();
            [DispId(0x68)]
            void Navigate([In] string Url, [In] ref object flags, [In] ref object targetFrameName, [In] ref object postData, [In] ref object headers);
            [DispId(-550)]
            void Refresh();
            [DispId(0x69)]
            void Refresh2([In] ref object level);
            [DispId(0x6a)]
            void Stop();
            [DispId(200)]
            object Application { [return: MarshalAs(UnmanagedType.IDispatch)] get; }
            [DispId(0xc9)]
            object Parent { [return: MarshalAs(UnmanagedType.IDispatch)] get; }
            [DispId(0xca)]
            object Container { [return: MarshalAs(UnmanagedType.IDispatch)] get; }
            [DispId(0xcb)]
            object Document { [return: MarshalAs(UnmanagedType.IDispatch)] get; }
            [DispId(0xcc)]
            bool TopLevelContainer { get; }
            [DispId(0xcd)]
            string Type { get; }
            [DispId(0xce)]
            int Left { get; set; }
            [DispId(0xcf)]
            int Top { get; set; }
            [DispId(0xd0)]
            int Width { get; set; }
            [DispId(0xd1)]
            int Height { get; set; }
            [DispId(210)]
            string LocationName { get; }
            [DispId(0xd3)]
            string LocationURL { get; }
            [DispId(0xd4)]
            bool Busy { get; }
            [DispId(300)]
            void Quit();
            [DispId(0x12d)]
            void ClientToWindow(out int pcx, out int pcy);
            [DispId(0x12e)]
            void PutProperty([In] string property, [In] object vtValue);
            [DispId(0x12f)]
            object GetProperty([In] string property);
            [DispId(0)]
            string Name { get; }
            [DispId(-515)]
            int HWND { get; }
            [DispId(400)]
            string FullName { get; }
            [DispId(0x191)]
            string Path { get; }
            [DispId(0x192)]
            bool Visible { get; set; }
            [DispId(0x193)]
            bool StatusBar { get; set; }
            [DispId(0x194)]
            string StatusText { get; set; }
            [DispId(0x195)]
            int ToolBar { get; set; }
            [DispId(0x196)]
            bool MenuBar { get; set; }
            [DispId(0x197)]
            bool FullScreen { get; set; }
            [DispId(500)]
            void Navigate2([In] ref object URL, [In] ref object flags, [In] ref object targetFrameName, [In] ref object postData, [In] ref object headers);
            [DispId(0x1f5)]
            NativeMethods.OLECMDF QueryStatusWB([In] NativeMethods.OLECMDID cmdID);
            [DispId(0x1f6)]
            void ExecWB([In] NativeMethods.OLECMDID cmdID, [In] NativeMethods.OLECMDEXECOPT cmdexecopt, ref object pvaIn, IntPtr pvaOut);
            [DispId(0x1f7)]
            void ShowBrowserBar([In] ref object pvaClsid, [In] ref object pvarShow, [In] ref object pvarSize);
            [DispId(-525)]
            WebBrowserReadyState ReadyState { get; }
            [DispId(550)]
            bool Offline { get; set; }
            [DispId(0x227)]
            bool Silent { get; set; }
            [DispId(0x228)]
            bool RegisterAsBrowser { get; set; }
            [DispId(0x229)]
            bool RegisterAsDropTarget { get; set; }
            [DispId(0x22a)]
            bool TheaterMode { get; set; }
            [DispId(0x22b)]
            bool AddressBar { get; set; }
            [DispId(0x22c)]
            bool Resizable { get; set; }
        }

		[ComImport, Guid("34A715A0-6587-11D0-924A-0020AFC7AC4D"), InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		internal interface DWebBrowserEvents2
		{
			[DispId(0x66)]
			void StatusTextChange([In] string text);
			[DispId(0x6c)]
			void ProgressChange([In] int progress, [In] int progressMax);
			[DispId(0x69)]
			void CommandStateChange([In] int command, [In] bool enable);
			[DispId(0x6a)]
			void DownloadBegin();
			[DispId(0x68)]
			void DownloadComplete();
			[DispId(0x71)]
			void TitleChange([In] string text);
			[DispId(0x70)]
			void PropertyChange([In] string szProperty);
			[DispId(250)]
			void BeforeNavigate2([In, MarshalAs(UnmanagedType.IDispatch)] object pDisp, [In] ref object URL, [In] ref object flags, [In] ref object targetFrameName, [In] ref object postData, [In] ref object headers, [In, Out] ref bool cancel);
			[DispId(0xfb)]
			void NewWindow2([In, Out, MarshalAs(UnmanagedType.IDispatch)] ref object pDisp, [In, Out] ref bool cancel);
			[DispId(0xfc)]
			void NavigateComplete2([In, MarshalAs(UnmanagedType.IDispatch)] object pDisp, [In] ref object URL);
			[DispId(0x103)]
			void DocumentComplete([In, MarshalAs(UnmanagedType.IDispatch)] object pDisp, [In] ref object URL);
			[DispId(0xfd)]
			void OnQuit();
			[DispId(0xfe)]
			void OnVisible([In] bool visible);
			[DispId(0xff)]
			void OnToolBar([In] bool toolBar);
			[DispId(0x100)]
			void OnMenuBar([In] bool menuBar);
			[DispId(0x101)]
			void OnStatusBar([In] bool statusBar);
			[DispId(0x102)]
			void OnFullScreen([In] bool fullScreen);
			[DispId(260)]
			void OnTheaterMode([In] bool theaterMode);
			[DispId(0x106)]
			void WindowSetResizable([In] bool resizable);
			[DispId(0x108)]
			void WindowSetLeft([In] int left);
			[DispId(0x109)]
			void WindowSetTop([In] int top);
			[DispId(0x10a)]
			void WindowSetWidth([In] int width);
			[DispId(0x10b)]
			void WindowSetHeight([In] int height);
			[DispId(0x107)]
			void WindowClosing([In] bool isChildWindow, [In, Out] ref bool cancel);
			[DispId(0x10c)]
			void ClientToHostWindow([In, Out] ref int cx, [In, Out] ref int cy);
			[DispId(0x10d)]
			void SetSecureLockIcon([In] int secureLockIcon);
			[DispId(270)]
			void FileDownload([In, Out] ref bool cancel);
			[DispId(0x10f)]
			void NavigateError([In, MarshalAs(UnmanagedType.IDispatch)] object pDisp, [In] ref object URL, [In] ref object frame, [In] ref object statusCode, [In, Out] ref bool cancel);
			[DispId(0xe1)]
			void PrintTemplateInstantiation([In, MarshalAs(UnmanagedType.IDispatch)] object pDisp);
			[DispId(0xe2)]
			void PrintTemplateTeardown([In, MarshalAs(UnmanagedType.IDispatch)] object pDisp);
			[DispId(0xe3)]
			void UpdatePageStatus([In, MarshalAs(UnmanagedType.IDispatch)] object pDisp, [In] ref object nPage, [In] ref object fDone);
			[DispId(0x110)]
			void PrivacyImpactedStateChange([In] bool bImpacted);
			[DispId(0x111)]
			void NewWindow3([In, Out, MarshalAs(UnmanagedType.IDispatch)] ref object pDisp, [In, Out] ref bool cancel, [In] int dwFlags, [In] ref object bstrUrlContext, [In] ref object bstrUrl);
		}
	}
	// this event sink declares the NewWindow3 event
	public class WebBrowserEvents : StandardOleMarshalObject, UnsafeNativeMethods.DWebBrowserEvents2, IDisposable
	{
		private AxHost.ConnectionPointCookie _cookie;
		public event EventHandler PrintTemplateTeardowned;
		public event EventHandler PrintTemplateInstanted;

		public WebBrowserEvents(WebBrowser wb)
		{
			_cookie = new AxHost.ConnectionPointCookie(wb.ActiveXInstance, this, typeof(UnsafeNativeMethods.DWebBrowserEvents2));
		}

		

		public void Dispose()
		{
			if(_cookie != null)
			{
				_cookie.Disconnect();
				_cookie = null;
			}
		}

		public void StatusTextChange([In] string text)
		{
			
		}

		public void ProgressChange([In] int progress, [In] int progressMax)
		{
			
		}

		public void CommandStateChange([In] int command, [In] bool enable)
		{
			
		}

		public void DownloadBegin()
		{
			
		}

		public void DownloadComplete()
		{
			
		}

		public void TitleChange([In] string text)
		{
			
		}

		public void PropertyChange([In] string szProperty)
		{
			
		}

		public void BeforeNavigate2([In, MarshalAs(UnmanagedType.IDispatch)] object pDisp, [In] ref object URL, [In] ref object flags, [In] ref object targetFrameName, [In] ref object postData, [In] ref object headers, [In, Out] ref bool cancel)
		{
			
		}

		public void NewWindow2([In, MarshalAs(UnmanagedType.IDispatch), Out] ref object pDisp, [In, Out] ref bool cancel)
		{
			
		}

		public void NavigateComplete2([In, MarshalAs(UnmanagedType.IDispatch)] object pDisp, [In] ref object URL)
		{
			
		}

		public void DocumentComplete([In, MarshalAs(UnmanagedType.IDispatch)] object pDisp, [In] ref object URL)
		{
			
		}

		public void OnQuit()
		{
			
		}

		public void OnVisible([In] bool visible)
		{
			
		}

		public void OnToolBar([In] bool toolBar)
		{
			
		}

		public void OnMenuBar([In] bool menuBar)
		{
			
		}

		public void OnStatusBar([In] bool statusBar)
		{
			
		}

		public void OnFullScreen([In] bool fullScreen)
		{
			
		}

		public void OnTheaterMode([In] bool theaterMode)
		{
			
		}

		public void WindowSetResizable([In] bool resizable)
		{
			
		}

		public void WindowSetLeft([In] int left)
		{
			
		}

		public void WindowSetTop([In] int top)
		{
			
		}

		public void WindowSetWidth([In] int width)
		{
			
		}

		public void WindowSetHeight([In] int height)
		{
			
		}

		public void WindowClosing([In] bool isChildWindow, [In, Out] ref bool cancel)
		{
			
		}

		public void ClientToHostWindow([In, Out] ref int cx, [In, Out] ref int cy)
		{
			
		}

		public void SetSecureLockIcon([In] int secureLockIcon)
		{
			
		}

		public void FileDownload([In, Out] ref bool cancel)
		{
			
		}

		public void NavigateError([In, MarshalAs(UnmanagedType.IDispatch)] object pDisp, [In] ref object URL, [In] ref object frame, [In] ref object statusCode, [In, Out] ref bool cancel)
		{
			
		}

		public void PrintTemplateInstantiation([In, MarshalAs(UnmanagedType.IDispatch)] object pDisp)
		{
			OnPrintTemplateInstanted();
		}

		private void OnPrintTemplateInstanted()
		{
			PrintTemplateInstanted?.Invoke(this, EventArgs.Empty); 
		}

		public void PrintTemplateTeardown([In, MarshalAs(UnmanagedType.IDispatch)] object pDisp)
		{
			OnPrintTemplateTeardowned();
		}

		private void OnPrintTemplateTeardowned()
		{
			PrintTemplateTeardowned?.Invoke(this, EventArgs.Empty);
		}

		public void UpdatePageStatus([In, MarshalAs(UnmanagedType.IDispatch)] object pDisp, [In] ref object nPage, [In] ref object fDone)
		{
			
		}

		public void PrivacyImpactedStateChange([In] bool bImpacted)
		{
			
		}

		public void NewWindow3([In, MarshalAs(UnmanagedType.IDispatch), Out] ref object pDisp, [In, Out] ref bool cancel, [In] int dwFlags, [In] ref object bstrUrlContext, [In] ref object bstrUrl)
		{
			
		}
	}
}
