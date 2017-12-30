using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;


namespace dingo_tray
{
    public partial class kindof_console : Form
    {
        int dingo_id;
        string dingo_defaults = "-port 53 -gdns:server 216.58.209.142 -h1";

        public kindof_console()
        {
            InitializeComponent();
            dingo_id = RunWithRedirect("dingo.exe", GetArguments(), dingo_output);

            StartPosition = FormStartPosition.Manual;
            Rectangle workingArea = Screen.GetWorkingArea(this);
            Location = new Point(workingArea.Right - Size.Width, workingArea.Bottom - Size.Height);

            ShowInTaskbar = false;
        }

        private string GetArguments() {
            string path = "dingo_tray.conf";
            if (!File.Exists(path))
            {
                tray_icon.ShowBalloonTip(5000, "Dingo DNS", "Не найден " + path + ", запуск dingo.exe с аргментами по умолчанию: " + dingo_defaults, ToolTipIcon.Info);
                return dingo_defaults;
            } else
            {
                using (StreamReader sr = new StreamReader("dingo_tray.conf"))
                {
                    return sr.ReadToEnd();
                }
            }
        }

        private void kindof_console_SizeChanged(object sender, EventArgs e)
        { 
            Visible = WindowState != FormWindowState.Minimized;
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            Visible = true;
            WindowState = FormWindowState.Normal;
        }

        // https://stackoverflow.com/a/415655
        private int RunWithRedirect(string cmdPath, string args, RichTextBox output)
        {
            if (!File.Exists(cmdPath))
            {
                tray_icon.ShowBalloonTip(10000, "Dingo DNS", "Не найден " + cmdPath + ", приложение, с надеждой в сердце и трепетным взглядом в будущее, закроется через 10 секунд...", ToolTipIcon.Error);
                Thread.Sleep(10000);
                Environment.Exit(1);
                return -1;
            }
            else
            {

                void proc_DataReceived(object sender, DataReceivedEventArgs e)
                {
                    if (!String.IsNullOrEmpty(e.Data)) ThreadSafeAppendText(output, e.Data + "\n");
                }

                var proc = new Process();
                proc.StartInfo.FileName = cmdPath;
                proc.StartInfo.Arguments = args;

                // set up output redirection
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.RedirectStandardError = true;
                proc.EnableRaisingEvents = true;
                proc.StartInfo.CreateNoWindow = true;
                proc.StartInfo.UseShellExecute = false;

                // see below for output handler
                proc.ErrorDataReceived += proc_DataReceived;
                proc.OutputDataReceived += proc_DataReceived;

                proc.Start();

                proc.BeginErrorReadLine();
                proc.BeginOutputReadLine();

                return proc.Id;
            }
        }

        private void ThreadSafeAppendText(RichTextBox rtb, string text)
        {
            Invoke((MethodInvoker)delegate ()
            {
               rtb.AppendText(text);
            });
        }

        private void kindof_console_FormClosed(object sender, FormClosedEventArgs e)
        {
            try {
                Process.GetProcessById(dingo_id).Kill();
            }
            catch { }
        }
    }
}
