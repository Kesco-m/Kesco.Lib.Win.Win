using System;
using Microsoft.Win32;

namespace Kesco.Lib.Win
{
	public class Registry
	{
		public static object GetValue(string name, object defaultValue)
		{
			string n = AppDomain.CurrentDomain.FriendlyName;
			n=n.Substring(0,n.Length-4);
			RegistryKey key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("Software\\Kesco\\"+n );
		    return key != null ? key.GetValue(name, defaultValue) : defaultValue;
		}

        public static void SetValue(string name, object value)
        {
            string n = AppDomain.CurrentDomain.FriendlyName;
            n = n.Substring(0, n.Length - 4);
            RegistryKey key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("Software\\Kesco\\" + n);
            if (key != null)
            {
                key.SetValue(name, value);
                key.Close();
            }
        }
	}
}
