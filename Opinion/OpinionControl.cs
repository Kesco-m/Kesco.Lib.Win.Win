using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kesco.Lib.Win.Opinion
{
	public partial class OpinionControl : UserControl
	{
		public OpinionControl()
		{
			InitializeComponent();
		}

		private void labelDislike_Click(object sender, EventArgs e)
		{
			SetRating(-1); 
		}

		private void labelLike_Click(object sender, EventArgs e)
		{
			SetRating(1);
		}

		private void SetRating(int r)
		{
			if(!isConnected)
				return;
			Task<bool> s = Task.Factory.StartNew<bool>(SetRatingBegin, r);
			Task<int> t = Task.Factory.ContinueWhenAny<bool,int>(new Task<bool>[] {s}, GetRatingBegin);
			Task con = Task.Factory.ContinueWhenAny<int>(new Task<int>[] { t }, EndGetRating);
		}

		private void OpinionControl_Load(object sender, EventArgs e)
		{
			//this.Enabled = Environment.IsConnectedErrors;
			//if(!Environment.IsConnectedErrors)
			//{
			//    Lib.Win.Options.Folder root = new Lib.Win.Options.Root();
			//    Environment.ConnectionStringErrors = root.OptionForced<string>("DS_errors").GetValue<string>();
			//}
			//if(Environment.IsConnectedErrors)
			//{
			//    this.Enabled = true;
			//    Task<int> t = Task.Factory.StartNew<int>(GetRating);
			//    Task con = Task.Factory.ContinueWhenAny<int>(new Task<int>[] { t }, EndGetRating);
			//}
		}

		public int GetRating()
		{
			if(!isConnected)
				return 0;
			using(SqlConnection cn = new SqlConnection(connectionString))
			using(SqlCommand cmd = new SqlCommand("SELECT dbo.fn_ИтоговаяОценкаСотрудника(@AppID, @EmpID)", cn))
			{
				cmd.Parameters.AddWithValue("@AppID", AppID);
				cmd.Parameters.AddWithValue("@EmpID", EmpID);
				try
				{
					cmd.Connection.Open();
					object obj = cmd.ExecuteScalar();
					if(obj is int)
						return (int)obj;
				}
				catch(SqlException sex)
				{
					Log.Logger.WriteEx(new Log.DetailedException(sex.Message, sex, cmd, Log.Priority.Error));
					Error.ErrorShower.OnShowError(this, sex.Message, "Error");
				}
				catch(Exception ex)
				{
					Log.Logger.WriteEx(ex);
				}
				finally
				{
					if(cmd.Connection.State != ConnectionState.Closed)
						cmd.Connection.Close();
				}
			}
			return 0;
		}

		public void EndGetRating(Task<int> t)
		{
			if(t.IsFaulted)
				SetStatus(0);
			else
				SetStatus(t.Result);
		}

		private void SetStatus(int status)
		{
			if(this.InvokeRequired)
			{
				this.BeginInvoke(new Action<int>(SetStatus), status);
				return;
			}
			if(status == 0)
			{
				labelLike.Text = "";
				buttonLike.ImageIndex = 0;
				labelDislike.Text = "";
				buttonDislike.ImageIndex = 2;
			}
			if(status > 0)
			{
				labelLike.Text = status.ToString();
				buttonLike.ImageIndex = 1;
				labelDislike.Text = "";
				buttonDislike.ImageIndex = 2;
			}
			if(status < 0)
			{
				labelLike.Text = "";
				buttonLike.ImageIndex = 0;
				labelDislike.Text = Math.Abs(status).ToString();
				buttonDislike.ImageIndex = 3;
			}
		}

		public string ConnectionString
		{
			get { return connectionString; }
			set
			{
				connectionString = value;
				isConnected = !string.IsNullOrEmpty(connectionString);
				if(isConnected)
					isConnected = TestConnection();
				if(isConnected)
				{
					this.Enabled = true;
					Task<int> t = Task.Factory.StartNew<int>(GetRating);
					Task con = Task.Factory.ContinueWhenAny<int>(new Task<int>[] { t }, EndGetRating);
				}
				else
					this.Enabled = false;
			}
		}

		public bool TestConnection()
		{
			using(var c = new SqlConnection(connectionString))
				try
				{
					c.Open();
					return true;
				}
				catch { }
				finally
				{
					if(c.State == ConnectionState.Open)
						c.Close();
				}
			return false;
		}

		public bool SetRatingBegin(object rat)
		{
			if(!isConnected || !(rat is int))
				return false;
			using(SqlConnection cn = new SqlConnection(connectionString))
			using(SqlCommand cmd = new SqlCommand("INSERT INTO vwОценкиИнтерфейса(КодИдентификатораОценки,Оценка) VALUES (@AppID, @Rating)", cn))
			{
				cmd.Parameters.AddWithValue("@AppID", AppID);
				cmd.Parameters.AddWithValue("@Rating", rat);
				try
				{
					cn.Open();
					return cmd.ExecuteNonQuery() > 0;
				}
				catch(SqlException sex)
				{
					Log.Logger.WriteEx(new Log.DetailedException(sex.Message, sex, cmd, Log.Priority.Error));
					Error.ErrorShower.OnShowError(this, sex.Message, "Error");
				}
				catch(Exception ex)
				{
					Log.Logger.WriteEx(ex);
				}
				finally
				{
					if(cn.State != ConnectionState.Closed)
						cn.Close();
				}
			}
			return false;
		}

		public int EmpID;

		public int GetRatingBegin(Task<bool> task)
		{
			return GetRating();
		}

		public string AppID;

		private string connectionString;
		private bool isConnected;
	}
}
