using System;
using System.Runtime.InteropServices;

namespace Kesco.Lib.Win
{
	public delegate int HookProc(int nCode, IntPtr wParam, IntPtr lParam);
	/// <summary>
	/// Summary description for HookClass.
	/// </summary>
	public class HookClass
	{
		public static bool shiftPush;
		public static bool cntlPush;
		public static int WH_KEYBOARD_LL = 13;
		public static int hHook;

		[StructLayout(LayoutKind.Sequential)]
			public class LowLevelKeyboardStruct
		{
			public int vkCode;
			public int scanCode;
			public int flags;
			public int time;
			public int dwExtraInfo;
		}


		//This is the Import for the SetWindowsHookEx function.
		//Use this function to install a thread-specific hook.
		[DllImport("user32.dll",CharSet=CharSet.Auto, CallingConvention=CallingConvention.StdCall)]
		public static extern int SetWindowsHookEx(int idHook, HookProc lpfn,
			IntPtr hInstance, int threadId);

		//This is the Import for the UnhookWindowsHookEx function.
		//Call this function to uninstall the hook.
		[DllImport("user32.dll",CharSet=CharSet.Auto, CallingConvention=CallingConvention.StdCall)]
		public static extern bool UnhookWindowsHookEx(int idHook);

		//This is the Import for the CallNextHookEx function.
		//Use this function to pass the hook information to the next hook procedure in chain.
		[DllImport("user32.dll",CharSet=CharSet.Auto, CallingConvention=CallingConvention.StdCall)]
		public static extern int CallNextHookEx(int idHook, int nCode,
			IntPtr wParam, IntPtr lParam);
	}
}
