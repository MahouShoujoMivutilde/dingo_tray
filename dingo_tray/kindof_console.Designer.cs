namespace dingo_tray
{
    partial class kindof_console
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(kindof_console));
            this.tray_icon = new System.Windows.Forms.NotifyIcon(this.components);
            this.dingo_output = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // tray_icon
            // 
            this.tray_icon.Icon = ((System.Drawing.Icon)(resources.GetObject("tray_icon.Icon")));
            this.tray_icon.Text = "Dingo DNS";
            this.tray_icon.Visible = true;
            this.tray_icon.DoubleClick += new System.EventHandler(this.notifyIcon1_DoubleClick);
            // 
            // dingo_output
            // 
            this.dingo_output.BackColor = System.Drawing.Color.Black;
            this.dingo_output.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dingo_output.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dingo_output.ForeColor = System.Drawing.Color.White;
            this.dingo_output.Location = new System.Drawing.Point(0, 0);
            this.dingo_output.Name = "dingo_output";
            this.dingo_output.ReadOnly = true;
            this.dingo_output.Size = new System.Drawing.Size(918, 441);
            this.dingo_output.TabIndex = 0;
            this.dingo_output.Text = "";
            // 
            // kindof_console
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(918, 441);
            this.Controls.Add(this.dingo_output);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "kindof_console";
            this.Opacity = 0.4D;
            this.Text = "Dingo DNS";
            this.TopMost = true;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.kindof_console_FormClosed);
            this.SizeChanged += new System.EventHandler(this.kindof_console_SizeChanged);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon tray_icon;
        private System.Windows.Forms.RichTextBox dingo_output;
    }
}

