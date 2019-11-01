using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

namespace Kesco.Lib.Win.EmployeeDialog
{
	/// <summary>
	/// Парсер сотрудников (для EmployeeBlock).
	/// </summary>
	public class EmployeeParser : IDisposable
	{
		protected int candidateCount = 0;
		private DataTable empTable;
		protected List<DataRow> candidateEmployee;

		public EmployeeParser(string connectionString, int placeID, int status) : base()
		{
			Load(connectionString, placeID, status);
		}

		public void Dispose()
		{
			if(empTable != null)
			{
				empTable.Dispose();
				empTable = null;
			}
		}

		#region Accessors

		public virtual int CandidateCount
		{
			get { return candidateCount; }
		}

		public virtual List<DataRow> CandidateEmployees
		{
			get { return candidateEmployee; }
		}
		#endregion

		public void Load(string connectionString, int placeID, int status)
		{
			EmployeeData data = new EmployeeData(connectionString);
			data.LoadTables(placeID, status);
			DataSet ds = data.DS;
			if(empTable != null)
				empTable.Dispose();
			empTable = null;
			if((ds != null) && (ds.Tables.Count > 0))
				empTable = ds.Tables[0];
			ds.Dispose();
		}

		public DataRow[] Parse(ref string input)
		{
			string[] bits = SplitToBits(input);
			candidateCount = 0;

			List<DataRow> emps = new List<DataRow>();
			List<DataRow> candidateEmps = new List<DataRow>();
			for(int i = 0; i < bits.Length; i++)
			{
				string txt = bits[i].Trim();
				DataRow[] drs = ParseQuery(txt);

				if(drs == null || drs.Length <= 0)
					continue;

				candidateCount = drs.Length;
				for(int j = 0; j < candidateCount; j++)
				{
					DataRow dr = drs[j];
					candidateEmps.Add(dr);
				}
				if(candidateCount == 1)
				{
					DataRow dr = drs[0];
					emps.Add(dr);
				}
			}

			if(string.IsNullOrEmpty(input))
				candidateEmployee = null;
			else
			{
				if(candidateEmployee != null)
				{
					candidateEmployee.Clear();
					candidateEmployee = null;
				}
				candidateEmployee = new List<DataRow>(candidateEmps.OrderBy(x => (DateTime?)(x["ДатаРождения"] == DBNull.Value ? null : x["ДатаРождения"])).OrderBy(x => x["Отчество"]).OrderBy(x => x["Имя"]).OrderBy(x => x["Фамилия"]));
			}
			candidateEmps.Clear();
			candidateEmps = null;

			if(emps.Count == 1)
				input = emps[0]["Сотрудник"].ToString();

			return emps.ToArray();
		}

		private DataRow[] ParseQuery(string txt)
		{
			txt = Regex.Replace(txt, "[" + Regex.Escape(@"]\*+?|{[()^$#%'") + "]+", "", RegexOptions.IgnoreCase);
			txt = Regex.Replace(txt, "[.]+", " ", RegexOptions.IgnoreCase).Replace("  ", " ").Trim();

			if(txt.Length == 0)
				return null;

			string[] w = Regex.Split(txt.ToLower(), @"\s");
			string[] rw = w.Clone() as string[];

			int len = w.Length;
			if(len > 0 && len < 4)
			{
				const string f = "Фамилия";
				const string i = "Имя";
				const string o = "Отчество";
				const string l = "Login";

				switch(len)
				{
					case 1:
						return empTable.Rows.Cast<DataRow>().Where(x => ((string)x[l]).ToLower().StartsWith(w[0])
							|| ((string)x[f]).ToLower().StartsWith(w[0])
							|| ((string)x[i]).ToLower().StartsWith(w[0])).ToArray();
					case 2:
						return empTable.Rows.Cast<DataRow>().Where(x => ((string)x[f]).ToLower().StartsWith(w[0]) && ((string)x[i]).ToLower().StartsWith(rw[1])
							|| ((string)x[i]).ToLower().StartsWith(w[0]) && ((string)x[o]).ToLower().StartsWith(rw[1])
							|| ((string)x[i]).ToLower().StartsWith(w[0]) && ((string)x[f]).ToLower().StartsWith(rw[1])).ToArray();
					case 3:
						return empTable.Rows.Cast<DataRow>().Where(x => (((string)x[f]).ToLower().StartsWith(w[0]) && ((string)x[i]).ToLower().StartsWith(rw[1]) && ((string)x[o]).ToLower().StartsWith(w[2])) ||
							(((string)x[i]).ToLower().StartsWith(rw[0]) && ((string)x[o]).ToLower().StartsWith(w[1]) && ((string)x[f]).ToLower().StartsWith(rw[2]))).ToArray();
				}
			}

			return null;
		}

		public DataRow SetEmployee(object id)
		{
			var res = empTable.Rows.Cast<DataRow>().Where(x => x["КодСотрудника"].Equals(id));
			candidateEmployee = res.ToList();
			if(candidateEmployee.Count > 0)
				return candidateEmployee[0];
			else
				return null;
		}

		public string[] SplitToBits(string text)
		{
			return Regex.Split(text, "[,;]");
		}
	}
}