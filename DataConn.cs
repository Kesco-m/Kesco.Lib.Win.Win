using System;
using System.Reflection;

namespace Kesco.Lib.Win
{
    public class DataConn
    {
        // Берёт [ConnectionString по умолчанию] из сборки Data.dll,
        // если она загружена, если нет возвращает [null]
        public static string CnStr()
        {
            string ret = null;
            Type t = null;
            Assembly[] asms = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly t1 in asms)
            {
                t = t1.GetType("Kesco.Win.Data");
                if (t != null)
                    break;
            }
            if (t != null)
                ret = (string) t.GetField("cnStr").GetValue(null);
            return ret;
        }
    }
}
