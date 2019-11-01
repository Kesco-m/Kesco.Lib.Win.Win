using System;
using System.Windows.Forms;

namespace Kesco.Lib.Win
{
    public class UpDownTextBox : TextBox
    {
        private bool locked;
        private bool disposing;
        Timer timer;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.disposing = true;
                if (timer != null)
                {
                    timer.Tick -= timer_Tick;
                    timer.Stop();
                    timer.Dispose();
                    timer = null;
                }
            }
            base.Dispose(disposing);
        }

        protected override bool ProcessKeyMessage(ref Message m)
        {
            var key = (Keys)m.WParam.ToInt32();
            if (key == Keys.Up || key == Keys.Down)
            {
                OnKeyDown(new KeyEventArgs(key));
                return true;
            }

            return base.ProcessKeyMessage(ref m);
        }

        protected override void WndProc(ref Message m)
        {
            try
            {
                if (timer != null && m.Msg == 0x0100)
                {
                    timer.Stop();
                    ReadOnly = true;
                    timer.Interval = 350;
                    timer.Start();
                }
            }
            catch
            {
            }
            base.WndProc(ref m);
        }

        public bool Lock()
        {
            if (!locked)
            {
                locked = true;
                return true;
            }
            return false;
        }

        public void Unlock()
        {
            locked = false;
        }

        public void EnableForInput()
        {
            if (timer == null)
            {
                timer = new Timer();
                timer.Tick += timer_Tick;
            }
            else
                timer.Stop();
            ReadOnly = true;
            timer.Interval = 350;
            timer.Start();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (Disposing || IsDisposed)
                return;
            var ti = sender as Timer;
            if (ti != null)
            {
                ti.Tick -= timer_Tick;
                if (ti == timer)
                    timer = null;
                ti.Stop();
                ti.Dispose();
                if (!disposing)
                {
                    ReadOnly = false;
                    Focus();
                }
                ti = null;
            }
        }
    }
}