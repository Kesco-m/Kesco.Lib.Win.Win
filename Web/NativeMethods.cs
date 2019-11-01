namespace Kesco.Lib.Win.Web
{
    public static class NativeMethods
    {
        public enum OLECMDF
        {
            OLECMDF_DEFHIDEONCTXTMENU = 0x20,
            OLECMDF_ENABLED = 2,
            OLECMDF_INVISIBLE = 0x10,
            OLECMDF_LATCHED = 4,
            OLECMDF_NINCHED = 8,
            OLECMDF_SUPPORTED = 1
        }

		public enum OLECMDID
		{
			OLECMDID_PRINT = 6,
			OLECMDID_PAGESETUP = 8,
			OLECMDID_CUT = 11,
			OLECMDID_COPY = 12,
			OLECMDID_PASTE = 13,
			OLECMDID_PASTESPECIAL = 14,
			OLECMDID_SELECTALL = 17
		}

        public enum OLECMDEXECOPT
        {
            OLECMDEXECOPT_DODEFAULT = 0,
            OLECMDEXECOPT_DONTPROMPTUSER = 2,
            OLECMDEXECOPT_PROMPTUSER = 1,
            OLECMDEXECOPT_SHOWHELP = 3
        }
    }
}
