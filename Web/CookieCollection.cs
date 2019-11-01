using System.Collections;
using System.Text.RegularExpressions;

namespace Kesco.Lib.Win.Web
{
	/// <summary>
	/// Коллекция куков
	/// </summary>
	public class CookieCollection : IEnumerable
	{
		private ArrayList cookies;

		public CookieCollection()
		{
			cookies = new ArrayList();
		}

		public CookieCollection(string cookieStr) : this()
		{
			Fill(cookieStr);
		}

		public CookieEnumerator GetEnumerator()			// non-IEnumerable version
		{
			return new CookieEnumerator(this);
		}

		IEnumerator IEnumerable.GetEnumerator()			// IEnumerable version
		{
			return (IEnumerator) new CookieEnumerator(this);
		}

		// Внутренний класс, реализующий интерфейс IEnumerator

		public class CookieEnumerator : IEnumerator
		{
			private int position = -1;
			CookieCollection cc;

			public CookieEnumerator(CookieCollection cc)
			{
				this.cc = cc;
			}

			public bool MoveNext()
			{
				if (position < cc.cookies.Count - 1)
				{
					position++;
					return true;
				}
				else
					return false;
			}

			public void Reset()
			{
				position = -1;
			}

			public Cookie Current				// non-IEnumerator version. type-safe
			{
				get { return (Cookie)cc.cookies[position]; }
			}

			object IEnumerator.Current			// IEnumerator version. returns object
			{
				get { return cc.cookies[position]; }
			}
		}

		public void Add(Cookie cookie)
		{
			cookies.Add(cookie);
		}

		public void Fill(string cookieStr)
		{
			Clear();

			if (cookieStr != null)
			{

				Regex r = new Regex("(?<name>[^;=]+)=(?<value>[^;]*)((; )|$)");
				Match m =  r.Match(cookieStr);

				while (m.Success) 
				{
					string name = m.Groups["name"].Value.Trim();
					string val = m.Groups["value"].Value.Trim();

					Cookie cookie = new Cookie(name, val);
					Add(cookie);

					m = m.NextMatch();
				}
			}
		}

		public Cookie GetCookie(string name)
		{
			foreach (Cookie c in cookies)
				if (c.Name == name)
					return c;

			return null;
		}

		public int Count
		{
			get { return cookies.Count; }
		}

		public void Clear()
		{
			cookies.Clear();
		}
	}
}