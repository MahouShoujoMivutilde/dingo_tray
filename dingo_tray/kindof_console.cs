﻿using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace dingo_tray
{
    public partial class kindof_console : Form
    {
        int dingo_id;
        string dingo_defaults = "-port 53 -gdns:server 216.58.209.142 -h1";

        public kindof_console()
        {
            InitializeComponent();

            StartPosition = FormStartPosition.Manual;
            Rectangle workingArea = Screen.GetWorkingArea(this);
            Location = new Point(workingArea.Right - Size.Width, workingArea.Bottom - Size.Height);

            ShowInTaskbar = false;
            WindowState = FormWindowState.Minimized;

            string dingo_args = GetArguments();

            // если какое-то приложение занимает порт из аргументов и содержит в имени dingo - убить (на случай аварийного завершения проги раньше)
            foreach (var p in ProcessPorts.ProcessPortMap.FindAll(x => x.PortNumber == GetPort(dingo_args)))
            {
                var possibly_dingo = Process.GetProcessById(p.ProcessId);
                if (Path.GetFileName(possibly_dingo.MainModule.FileName).ToLower().Contains("dingo")) {
                    possibly_dingo.Kill();
                }
                break;
            }
            dingo_id = RunWithRedirect("dingo.exe", dingo_args, dingo_output);
        }

        private string GetArguments() {
            string path = "dingo_tray.conf";
            if (!File.Exists(path))
            {
                //tray_icon.ShowBalloonTip(5000, "Dingo DNS", "Не найден " + path + ", запуск dingo.exe с аргументами по умолчанию: " + dingo_defaults, ToolTipIcon.Info);
                return dingo_defaults;
            } else
            {
                using (var sr = new StreamReader("dingo_tray.conf"))
                {
                    return sr.ReadToEnd();
                }
            }
        }

        private int GetPort(string args)
        {
            var port_patt = new Regex(@"-port \d+", RegexOptions.IgnoreCase);
            int port = 53;
            if (port_patt.Match(args).Success)
            {
                port = int.Parse(Regex.Replace(port_patt.Match(args).Value, @"[^\d+]", ""));
            }
            return port;
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

                if (rtb.Lines.Length > 500) DeleteLine(0, rtb); // экономим память, типа

                rtb.SelectionStart = rtb.Text.Length;
                rtb.ScrollToCaret();
            });
        }

        // https://stackoverflow.com/a/4482942
        private void DeleteLine(int a_line, RichTextBox richTextBox)
        {
            int start_index = richTextBox.GetFirstCharIndexFromLine(a_line);
            int count = richTextBox.Lines[a_line].Length;

            // Eat new line chars
            if (a_line < richTextBox.Lines.Length - 1)
            {
                count += richTextBox.GetFirstCharIndexFromLine(a_line + 1) -
                    ((start_index + count - 1) + 1);
            }

            richTextBox.Text = richTextBox.Text.Remove(start_index, count);
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
